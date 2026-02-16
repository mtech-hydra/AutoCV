using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;
using AutoCV.Infrastructure.Ai;
using AutoCV.Web.Data;
using AutoCV.Web.Fakes;
using AutoCV.Web.Services;

using Microsoft.EntityFrameworkCore;

Console.WriteLine("=== AutoCV Console Runner ===");

// --------------------
// CONFIG
// --------------------

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var cs = context.Configuration.GetConnectionString("MainDb");

        Console.WriteLine("---- CONNECTION STRING DEBUG ----");
        Console.WriteLine(cs ?? "NULL");
        Console.WriteLine("--------------------------------");

        services.AddDbContext<AutoCvDbContext>(options =>
            options.UseSqlServer(cs));

        // Fake/demo services
        services.AddSingleton<IJobAdSource, FakeJobAdSource>();
        services.AddSingleton<ICandidateProfileSource, FakeCandidateProfileSource>();
        services.AddSingleton<ICvGenerator, FakeCvGenerator>();
        services.AddSingleton<IGeneratedDocumentWriter, FakeDocumentWriter>();
        services.AddSingleton<HappyPathRunner>();

        services.AddHttpClient();

        var huggingFaceApiKey =
            Environment.GetEnvironmentVariable("HuggingFaceApiKey") ?? "<!-- no key found -->";

        services.AddSingleton<IAiContentGenerator>(sp =>
            new HuggingFaceAiProvider(
                huggingFaceApiKey,
                sp.GetRequiredService<HttpClient>()
            ));

        services.AddSingleton<CvGeneratorService>();
        services.AddSingleton<CoverLetterGeneratorService>();

        services.AddSingleton<ProfileLoader>();
        services.AddSingleton<JobAdLoader>();
    })
    .Build();


// --------------------
// RUN
// --------------------

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

var db = services.GetRequiredService<AutoCvDbContext>();

DbInitializer.Initialize(db);

Console.WriteLine("DB connected: " + db.Database.CanConnect());
Console.WriteLine("DB name: " + db.Database.GetDbConnection().Database);

var cvGenerator = services.GetRequiredService<CvGeneratorService>();
var coverLetterGenerator = services.GetRequiredService<CoverLetterGeneratorService>();

var profileLoader = services.GetRequiredService<ProfileLoader>();
var jobAdLoader = services.GetRequiredService<JobAdLoader>();

var profiles = profileLoader.LoadProfiles().ToList();
var jobAds = jobAdLoader.LoadJobAdsFromDirectory().ToList();

Console.WriteLine($"Profiles: {profiles.Count}");
Console.WriteLine($"Job ads: {jobAds.Count}");

foreach (var profile in profiles)
{
    foreach (var jobAd in jobAds)
    {
        Console.WriteLine($"Generating for {profile.Header} -> {jobAd.Title}");

        var cvMd = await cvGenerator.GenerateMarkdownAsync(profile, jobAd);
        cvGenerator.SaveMarkdown(profile, cvMd, jobAd);

        var coverMd = await coverLetterGenerator.GenerateMarkdownAsync(profile, jobAd);
        coverLetterGenerator.SaveMarkdown(profile, coverMd, jobAd);

        bool exists = await db.Errand.AnyAsync(e => e.Title == jobAd.Title);

        if (!exists)
        {
            db.Errand.Add(new JobAdEntity
            {
                Title = jobAd.Title,
                Status = "Generated",
                CreatedDate = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }
    }
}

Console.WriteLine("=== DONE ===");

using AutoCV.Contracts.Dtos;
// NOTE:
// EF Core migrations intentionally disabled.
// Database schema will be introduced later.
// this comment is here to prevent "why is nothing migrating" headache?

using AutoCV.Contracts.Interfaces;
using AutoCV.Infrastructure.Ai;
using AutoCV.Web.Data;
using AutoCV.Web.Fakes;
using AutoCV.Web.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

foreach (var kv in builder.Configuration.AsEnumerable())
{
    if (kv.Key.Contains("ConnectionStrings"))
        Console.WriteLine($"{kv.Key} = {kv.Value}");
}

// Add services to the container.
builder.Services.AddRazorPages();

string cs = builder.Configuration.GetConnectionString("MainDb");
builder.Services.AddDbContext<AutoCvDbContext>(options =>
    options.UseSqlServer(cs));

Console.WriteLine("---- CONNECTION STRING DEBUG ----");
Console.WriteLine(cs == null ? "NULL" : $"Length: {cs.Length}");
Console.WriteLine(cs);
Console.WriteLine("--------------------------------");

builder.Services.AddSingleton<IJobAdSource, FakeJobAdSource>();
builder.Services.AddSingleton<ICandidateProfileSource, FakeCandidateProfileSource>();
builder.Services.AddSingleton<ICvGenerator, FakeCvGenerator>();
builder.Services.AddSingleton<IGeneratedDocumentWriter, FakeDocumentWriter>();
builder.Services.AddSingleton<HappyPathRunner>();

builder.Services.AddHttpClient();

var huggingFaceApiKey = Environment.GetEnvironmentVariable("HuggingFaceApiKey") ?? "<!-- no key found -->";
builder.Services.AddSingleton<IAiContentGenerator>(sp =>
    new HuggingFaceAiProvider(huggingFaceApiKey, sp.GetRequiredService<HttpClient>())
    //new DummyAiProvider(huggingFaceApiKey, sp.GetRequiredService<HttpClient>())
);

// CV and Cover Letter services
builder.Services.AddSingleton<CvGeneratorService>();
builder.Services.AddSingleton<CoverLetterGeneratorService>();

// Other services like DbContext
builder.Services.AddDbContext<AutoCvDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDb"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapGet("/happy", async (HappyPathRunner runner) =>
{
    await runner.RunAsync("job-123");
    return Results.Ok("Happy path executed");
});

app.MapGet("/health/db", async () =>
{
    var connStr = builder.Configuration["ConnectionStrings:MainDb"];

    if (string.IsNullOrWhiteSpace(connStr))
        return Results.Problem("Connection string not configured");

    await using var conn = new SqlConnection(connStr);
    await conn.OpenAsync();

    return Results.Ok("DB OK");
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Initialize DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutoCvDbContext>();
    DbInitializer.Initialize(db);

    Console.WriteLine("Database debug: " + db.Database.CanConnect());
    Console.WriteLine("Database debug: " + db.Database.GetDbConnection().Database);

    var cvGenerator = scope.ServiceProvider.GetRequiredService<CvGeneratorService>();
    // Placeholder old:
    // var coverLetterGenerator = new CoverLetterGeneratorService(app.Configuration);
    //var cvGenerator = new CvGeneratorService(app.Configuration);
    var coverLetterGenerator = scope.ServiceProvider.GetRequiredService<CoverLetterGeneratorService>();

    var profileLoader = new ProfileLoader(app.Configuration);
    var profiles = profileLoader.LoadProfiles().ToList();
    
    var jobAdLoader = new JobAdLoader(builder.Configuration);
    var jobAds = jobAdLoader.LoadJobAdsFromDirectory().ToList();


    // Example: generate a cover letter for each profile for each job ad
    foreach (var profile in profiles)
    {
        // Load job ads from DB
        //var jobAds = scope.ServiceProvider.GetRequiredService<AutoCvDbContext>().Errand.ToList();

        foreach (var jobAd in jobAds)
        { 
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
}


app.MapRazorPages();

app.Run();

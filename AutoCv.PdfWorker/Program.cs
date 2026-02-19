namespace AutoCv.PdfWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind settings
            builder.Services.Configure<PdfWorkerOptions>(
                builder.Configuration.GetSection("PdfWorker")
            );

            builder.Services.AddSingleton<PdfRenderer>();
            builder.Services.AddHostedService<PdfWorkerService>();

            var app = builder.Build();
            app.Run();

        }
    }
}

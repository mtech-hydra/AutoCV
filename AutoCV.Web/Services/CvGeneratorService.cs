using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;

namespace AutoCV.Web.Services
{
    public class CvGeneratorService
    {
        private readonly IAiContentGenerator _ai;

        public CvGeneratorService(IAiContentGenerator ai)
        {
            _ai = ai;
        }

        // New async version calling HuggingFace
        public async Task<string> GenerateMarkdownAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            // Determine file path
            var baseDir = Path.Combine("D:\\Nextcloud\\AutoCV\\JobAds",
                string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars())));
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            var filePath = Path.Combine(baseDir, $"cv.md");

            // If file already exists, skip AI
            if (File.Exists(filePath))
            {
                Console.WriteLine($"CV already exists for {profile.ProfileId} / {jobAd.Title}, skipping AI.");
                return File.ReadAllText(filePath);  // Optionally return existing content
            }

            // Otherwise, call AI
            var markdown = await _ai.GenerateCvAsync(profile, jobAd);

            // Save file
            File.WriteAllText(filePath, markdown);
            Console.WriteLine($"Saved CV to {filePath}");
            return markdown;
        }

        public void SaveMarkdown(CandidateProfileDto profile, string markdown, JobAdDto jobAd)
        {
            //var dir = "D:\\Nextcloud\\AutoCV\\Generated"; // or inject via config
            var dir = Path.Combine("D:\\Nextcloud\\AutoCV\\JobAds",
                string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars())));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string fileName = profile.ProfileId;
            if (jobAd != null)
                fileName += "_" + string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars()));

            var filePath = Path.Combine(dir, $"cv.md");
            File.WriteAllText(filePath, markdown);
            Console.WriteLine($"Saved CV to {filePath}");
        }
    }
}

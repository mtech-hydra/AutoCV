using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;
using AutoCV.Web.Data;
using System.Text;

namespace AutoCV.Web.Services
{
    public class CoverLetterGeneratorService
    {
        private readonly IAiContentGenerator _ai;

        public CoverLetterGeneratorService(IAiContentGenerator ai)
        {
            _ai = ai;
        }

        public async Task<string> GenerateMarkdownAsync(CandidateProfileDto profile, JobAdDto jobAd)
        {
            var baseDir = Path.Combine("D:\\Nextcloud\\AutoCV\\JobAds",
                string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars())));
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            var filePath = Path.Combine(baseDir, $"cover.md");

            // Skip AI if file exists
            if (File.Exists(filePath))
            {
                Console.WriteLine($"Cover letter already exists for {profile.ProfileId} / {jobAd.Title}, skipping AI.");
                return File.ReadAllText(filePath);  // Optionally return existing content
            }

            var markdown = await _ai.GenerateCoverLetterAsync(profile, jobAd);

            File.WriteAllText(filePath, markdown);
            Console.WriteLine($"Saved cover letter to {filePath}");
            return markdown;
        }

        public void SaveMarkdown(CandidateProfileDto profile, string markdown, JobAdDto jobAd)
        {
            //var dir = "D:\\Nextcloud\\AutoCV\\Generated"; // or inject via config
            var dir = Path.Combine("D:\\Nextcloud\\AutoCV\\JobAds",
                string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars())));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string fileName = profile.ProfileId + "_" + string.Join("_", jobAd.Title.Split(Path.GetInvalidFileNameChars()));
            var filePath = Path.Combine(dir, $"cover.md");
            File.WriteAllText(filePath, markdown);
            Console.WriteLine($"Saved cover letter to {filePath}");
        }
    }

}

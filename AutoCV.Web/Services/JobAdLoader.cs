using AutoCV.Contracts.Dtos;
using AutoCV.Web.Data;

namespace AutoCV.Web.Services
{
    using AutoCV.Contracts.Dtos;
    using Microsoft.Extensions.Configuration;

    public class JobAdLoader
    {
        private readonly string _jobAdsDirectory;

        public JobAdLoader(IConfiguration config)
        {
            _jobAdsDirectory = config["JobAdsDirectory"] ?? "D:\\Nextcloud\\AutoCV\\JobAds";
        }

        /// <summary>
        /// Loads all job ads from filesystem
        /// </summary>
        public IEnumerable<JobAdDto> LoadJobAdsFromDirectory()
        {
            if (!Directory.Exists(_jobAdsDirectory))
                yield break;

            foreach (var subdir in Directory.GetDirectories(_jobAdsDirectory))
            {
                var title = Path.GetFileName(subdir);

                // Read the .md file in the folder
                var mdFiles = Directory.GetFiles(subdir, "ad.md");
                if (mdFiles.Length == 0)
                    continue; // skip empty folders

                var content = File.ReadAllText(mdFiles[0]);

                yield return new JobAdDto
                {
                    Title = title,
                    Description = content
                };
            }
        }
    }

}

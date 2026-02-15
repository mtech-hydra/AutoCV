using AutoCV.Contracts.Dtos;
using System.Text.Json;

namespace AutoCV.Web.Services
{
    public class ProfileLoader
    {
        private readonly IConfiguration _config;

        public ProfileLoader(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<CandidateProfileDto> LoadProfiles()
        {
            var dir = _config["ProfilesDirectory"];
            Console.WriteLine($"Loading profiles from {dir}");
            if (!Directory.Exists(dir))
                yield break;

            foreach (var file in Directory.GetFiles(dir, "*.json"))
            {
                var json = File.ReadAllText(file);
                CandidateProfileDto? profile = null;

                try
                {
                    profile = JsonSerializer.Deserialize<CandidateProfileDto>(json);
                }
                catch
                {
                    Console.WriteLine($"Failed to read profile {file}");
                }

                if (profile != null)
                    yield return profile;
            }
        }
    }
}

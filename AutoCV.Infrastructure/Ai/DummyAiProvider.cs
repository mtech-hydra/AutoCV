using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace AutoCV.Infrastructure.Ai
{
    public class DummyAiProvider : IAiContentGenerator
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;

        public DummyAiProvider(string apiKey, HttpClient http)
        {
            _apiKey = apiKey;
            _http = http;
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GenerateCvAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            return profile.Header + profile.FreeText;
        }

        public async Task<string> GenerateCoverLetterAsync(CandidateProfileDto profile, JobAdDto jobAd)
        {
            return "Job description: " + jobAd.Description + "\n\n" + profile.Header + "I am interested in this position and I am highly skilled for this job.\n\n" + profile.Signature;
        }

        private class HfTextResult
        {
            public string GeneratedText { get; set; } = "";
        }
    }
}

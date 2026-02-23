using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AutoCV.Infrastructure.Ai
{
    public class HuggingFaceAiProvider : IAiContentGenerator
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;

        public HuggingFaceAiProvider(string apiKey, HttpClient http)
        {
            _apiKey = apiKey;
            _http = http;
            //_http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            // Remove usage of 'config' since it does not exist in this context.
            // Set default values directly. ToDo: Should be in config.
            _apiUrl = "https://router.huggingface.co/v1/chat/completions";
            _model = "deepseek-ai/DeepSeek-V3-0324";
        }
        private string BuildCvPrompt(CandidateProfileDto profile, JobAdDto? jobAd)
        {
            var prompt = $"Skapa CV headline. Svara utan förklaring. Direkt svar. ";
            if (jobAd != null)
                prompt += $"Relevant till:\n{jobAd.Title}\n{jobAd.Description}\n";
            prompt += "Max 80 words.";
            return prompt;
        }

        private string BuildCoverLetterPrompt(CandidateProfileDto profile, JobAdDto? jobAd)
        {
            var prompt = $"Write cover letter. Use scandinavian brutalism. Svara utan förklaring. Direkt svar.\n";
            if (jobAd != null)
                prompt += $"Relevant till:\n{jobAd.Title}\n{jobAd.Description}\n";
            prompt += "Max 120 ord.";
            return prompt;
        }

        public async Task<string> GenerateCvAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            var messages = new List<object>
        {
            new { role = "system", content = "You are a CV generator assistant." },
            new { role = "user", content = BuildCvPrompt(profile, jobAd) }
        };

            var payload = new
            {
                model = _model,
                messages = messages,
                max_new_tokens = 200
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = JsonContent.Create(payload);

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"HuggingFace API error: {response.StatusCode}\n{content}");
                return "Error: HuggingFace API failed.";
            }

            // Parse JSON result
            using var doc = JsonDocument.Parse(content);
            var text = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return profile.Header + " " + (text ?? "Error: Failed to generate CV.") + profile.FreeText;
        }

        public async Task<string> GenerateCoverLetterAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            var messages = new List<object>
        {
            new { role = "system", content = "You are a CV generator assistant." },
            new { role = "user", content = BuildCoverLetterPrompt(profile, jobAd) }
        };

            var payload = new
            {
                model = _model,
                messages = messages,
                max_new_tokens = 200
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = JsonContent.Create(payload);

            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"HuggingFace API error: {response.StatusCode}\n{content}");
                return "Error: HuggingFace API failed.";
            }

            // Parse JSON result
            using var doc = JsonDocument.Parse(content);
            var text = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return profile.Header + " " + (text ?? "Error: Failed to generate CV.") + profile.Signature;
        }

        private class HfTextResult
        {
            public string GeneratedText { get; set; } = "";
        }
    }
}

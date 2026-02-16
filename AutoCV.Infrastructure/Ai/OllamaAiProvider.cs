using AutoCV.Contracts.Dtos;
using AutoCV.Contracts.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace AutoCV.Infrastructure.Ai
{
    public class OllamaAiProvider : IAiContentGenerator
    {
        private readonly HttpClient _http;
        private readonly string _apiUrl;
        private readonly string _model;

        public OllamaAiProvider(HttpClient http, string model = "llama3.2:latest", string apiUrl = "http://localhost:11434/v1/completions")
        {
            _http = http;
            _model = model;
            _apiUrl = apiUrl.TrimEnd('/');
        }

        private string BuildCvPrompt(CandidateProfileDto profile, JobAdDto? jobAd)
        {
            var prompt = $"Skapa CV headline. Svara utan förklaring. Direkt svar. ";
            if (jobAd != null)
                prompt += $"Relevant till:\n{jobAd.Title}\n{jobAd.Description}\n";
            prompt += "About 20 words.";
            return prompt;
        }

        private string BuildCoverLetterPrompt(CandidateProfileDto profile, JobAdDto? jobAd)
        {
            var prompt = $"**Instructions:**\r\n" +
                $"1. Address key requirements from the job ad\r\n" +
                $"2. Show enthusiasm for the role and company\r\n" +
                $"3. Important: Connect your experience to their needs\r\n" +
                $"4. Keep it not too formal, engaging\r\n" +
                $"5. Structure: Opening, 2-3 body paragraphs, closing\r\n" +
                $"6. Write simple sentences, use any of these per sentence " +
                $"(implication, 1 subjunctive clause, 1 main clause, 1 subordinate clause per sentence). " +
                $"Each sentence contains proof from CV.\r\n" +
                $"7. For each statement provide proof in form of an example from CV\r\n" +
                $"8. Keep it to 1 page equivalent and max 300 words\r\n" +
                $"9. Include the fact that (max 50 words):\r\n\r\n" +
                $"Those of us with degrees in computer science or other science-based disciplines " +
                $"have a specific way of thinking, where when there's a problem and we need to get from" +
                $" A to B, we focus solely on how to conduct that reasoning.\r\n\r\n" +
                $"Many professionals have can't see the whole problem. " +
                $"We, for example, are able to think much more broadly, " +
                $"combining mechanisms from different branches of science or different ways of thinking. " +
                $"We can think outside the box a bit. " +
                $"\"What if that were the case? And what are the shortcomings we see?\" " +
                $"And many people often have trouble with that. They often think very schematically," +
                $" so this kind of thinking is very helpful. " +
                $"It's a great feeling that I can bring something new that others can't come up with on their own." +
                $"\r\n\r\n10. the individual being discussed is ambitious, driven, and quick to learn, " +
                $"but these aspects need to be highlighted more personally. " +
                $"draw on personal experiences and traits to create a richer narrative that better illustrates " +
                $"who the individual is.\r\n" +
                $"11. separate different areas of focus in presentation, such as skills and personality traits: " +
                $"a strategy to clarify the person’s attributes and fit for jobs in the tech space.\r\n" +
                $"12. Highlight that the individual has positive attitude to personal and professional development.\r\n\n";
            if (jobAd != null)
                prompt += $"\n\n**Annons**:\n\n{jobAd.Title}\n{jobAd.Description}\n";
            prompt += "Max 120 ord.";
            return prompt;
        }

        public async Task<string> GenerateCvAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            string prompt = BuildCvPrompt(profile, jobAd);

            var payload = new
            {
                model = _model,
                prompt = prompt,
                max_new_tokens = 200
            };

            var response = await _http.PostAsJsonAsync(_apiUrl, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ollama API error: {response.StatusCode}\n{content}");
                return "Error: Ollama API failed.";
            }

            try
            {
                using var doc = JsonDocument.Parse(content);
                var text = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("text")
                    .GetString();

                return profile.Header + " " + (text ?? "Error: Empty CV generated") + profile.FreeText;
            }
            catch
            {
                return "Error: Failed to parse Ollama response.";
            }
        }


        public async Task<string> GenerateCoverLetterAsync(CandidateProfileDto profile, JobAdDto? jobAd = null)
        {
            string prompt = BuildCoverLetterPrompt(profile, jobAd);

            var payload = new
            {
                model = _model,
                prompt = prompt,
                max_new_tokens = 300
            };

            var response = await _http.PostAsJsonAsync(_apiUrl, payload);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ollama API error: {response.StatusCode}\n{content}");
                return "Error: Ollama API failed.";
            }

            try
            {
                using var doc = JsonDocument.Parse(content);
                var text = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("text")
                    .GetString();

                return profile.Header + " " + (text ?? "Error: Empty CV generated") + profile.FreeText;
            }
            catch
            {
                return "Error: Failed to parse Ollama response.";
            }
        }
    }
}

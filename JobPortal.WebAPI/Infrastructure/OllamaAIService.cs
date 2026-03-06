using System.Net.Http.Json;

public class OllamaAIService : IAICoverLetterService
{
    private readonly HttpClient _httpClient;

    public OllamaAIService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> GenerateAsync(string jobDescription, string companyName, string cvSummary, string skills)
    {
        var prompt = $"Write a professional cover letter for {companyName} for the following job: {jobDescription}. Include candidate skills: {skills}. CV summary: {cvSummary}";
        var response = await _httpClient.PostAsJsonAsync("/api/generate", new { prompt });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        return result?.Text ?? "";
    }

    private class OllamaResponse { public string Text { get; set; } = ""; }
}
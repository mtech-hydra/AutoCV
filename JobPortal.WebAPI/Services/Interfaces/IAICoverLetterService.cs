public interface IAICoverLetterService
{
    Task<string> GenerateAsync(string jobDescription, string companyName, string cvSummary, string skills);
}
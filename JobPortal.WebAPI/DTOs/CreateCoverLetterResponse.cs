namespace JobPortal.WebAPI.DTOs
{
    public class CreateCoverLetterResponse
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsAIGenerated { get; set; }
        public string? AICustomPrompt { get; set; }
    }
}

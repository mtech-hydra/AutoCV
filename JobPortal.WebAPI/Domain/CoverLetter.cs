public class CoverLetter : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsAIGenerated { get; private set; }
    public string? AICustomPrompt { get; private set; }

    public CoverLetter() { }

    public CoverLetter(Guid userId, string title, string content, bool isAiGenerated, string? aiPrompt = null)
    {
        UserId = userId;
        Title = title;
        Content = content;
        IsAIGenerated = isAiGenerated;
        AICustomPrompt = aiPrompt;
    }

    public void UpdateContent(string content, string? aiPrompt = null)
    {
        Content = content;
        AICustomPrompt = aiPrompt;
        SetUpdated();
    }
}
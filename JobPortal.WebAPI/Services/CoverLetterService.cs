using JobPortal.WebAPI.DTOs;
using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CoverLetterService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CoverLetterService> _logger;
    public CoverLetterService(AppDbContext context, ILogger<CoverLetterService> logger) 
    {
        _context = context;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        // Initialize any dependencies here (e.g., database context, AI service client)
    }
    public Task<List<CoverLetter>> GetAllAsync(Guid userId)
    {
        return _context.CoverLetters
                       .Where(c => !c.IsDeleted)
                       .Where(c => c.UserId == userId)
                       .ToListAsync();
    }

    public async Task<CoverLetter?> GetByIdAsync(Guid id)
    {
        return await _context.CoverLetters
                             .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<CreateCoverLetterResponse> CreateAsync(Guid userId, CreateCoverLetterRequest request) {
        Console.WriteLine("[dbg] CreateCoverLetterAsync for user " + userId.ToString());
        var coverLetter = new CoverLetter(userId, request);
        _context.CoverLetters.Add(coverLetter);
        await _context.SaveChangesAsync();
        CreateCoverLetterResponse response = new CreateCoverLetterResponse
        {
            UserId = coverLetter.UserId,
            Title = coverLetter.Title,
            Content = coverLetter.Content,
        };
        Console.WriteLine("[dbg] CoverLetterResponse: " + response.ToString());
        return response;
    }

    public async Task<CoverLetter?> UpdateAsync(Guid id, UpdateCoverLetterRequest request)
    {
        // Fetch the existing cover letter
        var coverLetter = await _context.CoverLetters
                                        .FirstOrDefaultAsync(cl => cl.Id == id && !cl.IsDeleted);

        if (coverLetter is null)
        {
            Console.WriteLine($"[dbg] CoverLetter with Id {id} not found or is deleted.");
            return null; // Controller can return 404 if null
        }

        Console.WriteLine($"[dbg] Updating CoverLetter with Id {id} - Title: {request.Title}, Content Length: {request.Content.Length}, AI Prompt: {request.AICustomPrompt}");
        // Apply updates using the entity's Update method
        coverLetter.Update(request.Title, request.Content, request.AICustomPrompt);

        // Persist changes
        await _context.SaveChangesAsync();

        return coverLetter;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Attempting to delete cover letter with Id {Id}", id);
        var coverLetter = await _context.CoverLetters
                                        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coverLetter is null)
        {
            throw new KeyNotFoundException($"Cover letter with Id {id} not found.");
        }

        coverLetter.setDeleted();

        await _context.SaveChangesAsync();
        _logger.LogInformation("Cover letter with Id {Id} marked as deleted.", id);
    }

    public Task<string> GenerateAIAsync(Guid userId, GenerateCoverLetterRequest request)
    {
        // Dummy AI-generated text
        return Task.FromResult($"Generated cover letter for {request.CompanyName} using prompt: {request.AiPrompt ?? "default"}");
    }
}

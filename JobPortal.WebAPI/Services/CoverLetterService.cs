using JobPortal.WebAPI.DTOs;
using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class CoverLetterService
{
    private readonly AppDbContext _context;
    public CoverLetterService(AppDbContext context) 
    {
        _context = context;
        // Initialize any dependencies here (e.g., database context, AI service client)
    }
    public Task<List<CoverLetter>> GetAllAsync(Guid userId) {
        // ToDo: discuss benefits of returning IQueryable vs List here, and how to handle pagination/filtering/sorting in the future
        return _context.CoverLetters.Where(cl => cl.UserId == userId).ToListAsync();
    }

    public async Task<CoverLetter> GetByIdAsync(Guid id)
    {
        var coverLetter = await _context.CoverLetters
            .AsNoTracking()
            .FirstOrDefaultAsync(cl => cl.Id == id && !cl.IsDeleted);

        /*if (coverLetter == null)
        {
            throw new KeyNotFoundException($"Cover letter with id '{id}' was not found.");
        }*/

        return coverLetter; // Note: returning null if not found, controller can handle this and return 404
    }

    public async Task<CreateCoverLetterResponse> CreateAsync(Guid userId, CreateCoverLetterRequest request) {
        var coverLetter = new CoverLetter(request);
        _context.CoverLetters.Add(coverLetter);
        await _context.SaveChangesAsync();
        return new CreateCoverLetterResponse
        {
            UserId = coverLetter.UserId,
            Title = coverLetter.Title,
            Content = coverLetter.Content,
        };
    }

    public Task<CoverLetter> UpdateAsync(Guid id, UpdateCoverLetterRequest request) => Task.FromResult(new CoverLetter());

    public Task DeleteAsync(Guid id) => Task.CompletedTask;

    public Task<string> GenerateAIAsync(Guid userId, GenerateCoverLetterRequest request)
    {
        // Dummy AI-generated text
        return Task.FromResult($"Generated cover letter for {request.CompanyName} using prompt: {request.AiPrompt ?? "default"}");
    }
}

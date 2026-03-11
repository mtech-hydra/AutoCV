using JobPortal.WebAPI.DTOs;    
using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CVService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CVService> _logger;

    public CVService(AppDbContext context, ILogger<CVService> logger)
    {
        _context = context;
        // The following thing should never happen, nonetheless the runtime requires us to handle it.
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<List<CVProfile>> GetAllAsync() {
        return _context.CVProfiles
                       .Where(c => !c.IsDeleted)
                       .ToListAsync();
    }

    public async Task<CVProfile?> GetByIdAsync(Guid id)
    {
        return await _context.CVProfiles
                             .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<CvProfileResponse> CreateAsync(Guid userId, CreateCVRequest request)
    {
        var cvProfile = new CVProfile(userId, request.Title, request.Summary, request.Skills, request.Experience, request.Education);
        _context.CVProfiles.Add(cvProfile);
        await _context.SaveChangesAsync();  
        return new CvProfileResponse
        {
            Id = cvProfile.Id, Title = cvProfile.Title, Summary = cvProfile.Summary, Skills = cvProfile.Skills
        };
    }

    public async Task<CVProfile> UpdateAsync(Guid id, UpdateCVRequest request)
    {
        var cvProfile = await _context.CVProfiles
                                      .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (cvProfile is null)
        {
            _logger.LogWarning("CVProfile with Id {Id} not found or is deleted.", id);
            throw new KeyNotFoundException($"CVProfile with Id {id} not found.");
        }

        cvProfile.Update(request.Title, request.Summary, request.Skills, request.Experience, request.Education);

        await _context.SaveChangesAsync();

        return cvProfile;
    }

    public async Task DeleteAsync(Guid id)
    {
        var cvProfile = await _context.CVProfiles
                                      .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (cvProfile is null)
        {
            _logger.LogWarning("CVProfile with Id {Id} not found or already deleted.", id);
            throw new KeyNotFoundException($"CVProfile with Id {id} not found.");
        }

        cvProfile.setDeleted();

        await _context.SaveChangesAsync();

        _logger.LogInformation("CVProfile with Id {Id} soft-deleted.", id);
    }
}

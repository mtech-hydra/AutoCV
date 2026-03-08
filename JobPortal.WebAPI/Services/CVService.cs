using JobPortal.WebAPI.DTOs;    
using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CVService
{
    private readonly AppDbContext _context;
    public CVService(AppDbContext context)
    {
        _context = context;
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

    public Task<CVProfile> UpdateAsync(Guid id, UpdateCVRequest request) => Task.FromResult(new CVProfile());

    public Task DeleteAsync(Guid id) => Task.CompletedTask;
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortal.WebAPI.DTOs;
using JobPortal.WebAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class JobAdService
{
    private readonly AppDbContext _context;
    private readonly ILogger<JobAdService> _logger;
    public JobAdService(AppDbContext context, ILogger<JobAdService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public Task<List<JobAd>> GetAllActiveAsync()
    { // => Task.FromResult(new List<JobAd>());
        return _context.JobAds.Where(ad => ad.IsActive).Where(ad => !ad.IsDeleted).ToListAsync();
    }

    public async Task<JobAd> GetByIdAsync(Guid id)
    {//=> Task.FromResult(new JobAd());
        return await _context
            .JobAds
            .Where(ad => !ad.IsDeleted)
            .FirstOrDefaultAsync(ad => ad.Id == id);
    }
    public async Task<CreateJobAdRequestResponse> CreateAsync(Guid userId, CreateJobAdRequest request)
    { //  => Task.FromResult(new JobAd());
        var jobAd = new JobAd(
            userId: userId,
            title: request.Title,
            description: request.Description,
            companyName: request.CompanyName,
            location: request.Location,
            salaryRange: request.SalaryRange);
        _context.JobAds.Add(jobAd);
        await _context.SaveChangesAsync();

        return new CreateJobAdRequestResponse(jobAd);
    }
    public async Task<JobAd?> UpdateAsync(Guid id, UpdateJobAdRequest request)
    {
        // Fetch the existing job ad
        var jobAd = await _context.JobAds
                                  .FirstOrDefaultAsync(ad => ad.Id == id);

        if (jobAd is null)
            return null; // or throw new KeyNotFoundException($"JobAd with Id {id} not found.");

        // Use the Update method on the entity
        jobAd.Update(
            title: request.Title,
            description: request.Description,
            companyName: request.CompanyName,
            location: request.Location,
            salaryRange: request.SalaryRange,
            isActive: true); // ToDo: Assuming we want to make it active when updating

        await _context.SaveChangesAsync();

        return jobAd;
    }

    public async Task DeleteAsync(Guid id)
    {
        var jobAd = await _context.JobAds
                                  .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (jobAd is null)
        {
            _logger.LogWarning("JobAd with Id {Id} not found or already deleted.", id);
            throw new KeyNotFoundException($"JobAd with Id {id} not found.");
        }

        jobAd.setDeleted();

        await _context.SaveChangesAsync();

        _logger.LogInformation("JobAd with Id {Id} soft-deleted.", id);
    }
}

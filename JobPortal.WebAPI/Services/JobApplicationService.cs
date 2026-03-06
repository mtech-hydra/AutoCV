using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class JobApplicationService
{
    public Task<List<JobApplication>> GetAllAsync(Guid userId) => Task.FromResult(new List<JobApplication>());

    public Task<JobApplication> GetByIdAsync(Guid userId, Guid id) => Task.FromResult(new JobApplication());

    public Task<JobApplication> ApplyAsync(Guid userId, ApplyJobRequest request) => Task.FromResult(new JobApplication());

    public Task<JobApplication> UpdateStatusAsync(Guid userId, Guid id, UpdateApplicationStatusRequest request)
        => Task.FromResult(new JobApplication());
}

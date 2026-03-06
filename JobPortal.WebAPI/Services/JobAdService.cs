using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class JobAdService
{
    public Task<List<JobAd>> GetAllActiveAsync() => Task.FromResult(new List<JobAd>());

    public Task<JobAd> GetByIdAsync(Guid id) => Task.FromResult(new JobAd());

    public Task<JobAd> CreateAsync(Guid userId, CreateJobAdRequest request) => Task.FromResult(new JobAd());

    public Task<JobAd> UpdateAsync(Guid id, UpdateJobAdRequest request) => Task.FromResult(new JobAd());

    public Task DeleteAsync(Guid id) => Task.CompletedTask;
}

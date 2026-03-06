using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CVService
{
    public Task<List<CVProfile>> GetAllAsync() => Task.FromResult(new List<CVProfile>());

    public Task<CVProfile> GetByIdAsync(Guid id) => Task.FromResult(new CVProfile());

    public Task<CVProfile> CreateAsync(Guid userId, CreateCVRequest request) => Task.FromResult(new CVProfile());

    public Task<CVProfile> UpdateAsync(Guid id, UpdateCVRequest request) => Task.FromResult(new CVProfile());

    public Task DeleteAsync(Guid id) => Task.CompletedTask;
}

using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using JobPortal.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCV.Tests.JobAdServiceTests
{
    public class JobAdServiceTests
    {
        private readonly AppDbContext _context;
        private readonly JobAdService _service;
        public JobAdServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            _service = new JobAdService(_context);
        }

        [Fact]
        public async Task CreateJobAdAsync_ShouldCreateJobAdAndReturnResponse()
        {
            var request = new CreateJobAdRequest("Test Job", "Test Descripton", "ACME", "Universe", "10000000");
            var userId = Guid.NewGuid();

            var response = await _service.CreateAsync(userId, request);

            var jobInDb = await _context.JobAds.FirstOrDefaultAsync();
            Assert.NotNull(jobInDb);
            Assert.Equal("Test Job", jobInDb.Title);
        }
    }

    
}

using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AutoCV.Tests.CVProfiles
{
    public class CVProfileServiceTests
    {
        private readonly AppDbContext _context;
        private readonly CVService _service;

        public CVProfileServiceTests()
        {
            // Use a unique in-memory database for isolation
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Ensure the database is created
            _context.Database.EnsureCreated();

            _service = new CVService(_context);
        }

        [Fact]
        public async Task CreateCvProfileAsync_ShouldAddCvProfileAndReturnResponse()
        {
            // Arrange
            var request = new CreateCVRequest(
                "Test CV",
                "Test Summary",
                "C#, .NET",
                "Test experience",
                "Test education"
            );

            var userId = Guid.NewGuid();

            // Act
            var response = await _service.CreateAsync(userId, request);

            // Assert
            var cvInDb = await _context.CVProfiles.FirstOrDefaultAsync();
            Assert.NotNull(cvInDb);
            Assert.Equal("Test CV", cvInDb.Title);
            Assert.Equal("C#, .NET", cvInDb.Skills);

            Assert.Equal(response.Id, cvInDb.Id);
        }

        [Fact]
        public async Task GetCvProfileByIdAsync_ShouldReturnCvProfile_WhenExists()
        {
            // Arrange
            var cv = new CVProfile(
                Guid.NewGuid(),
                "Test CV",
                "Summary",
                "C#",
                "Some experience",
                "Some education"
            );

            _context.CVProfiles.Add(cv);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(cv.Id);

            // Assert
            Assert.NotNull(result);
            // ToDo: Testing Ids Assert.Equal(cv.Id, result.Id);
            Assert.Equal(cv.Title, result.Title);
        }

        [Fact]
        public async Task GetCvProfileByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }
    }
}

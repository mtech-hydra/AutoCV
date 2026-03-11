using JobPortal.WebAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AutoCV.Tests.CoverLetterTests
{
    public class CoverLetterServiceTests
    {
        private readonly AppDbContext _context;
        private readonly CoverLetterService _service;

        public CoverLetterServiceTests()
        {
            // Use a unique in-memory database for isolation
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Ensure the database is created
            _context.Database.EnsureCreated();

            _service = new CoverLetterService(_context);
        }

        [Fact]
        public async Task CreateCoverAsync_ShouldAddCoverLetterAndReturnResponse()
        {
            var request = new CreateCoverLetterRequest(
                "Test Cover Letter",
                "This is a test cover letter.",
                false
            );

            var userId = Guid.NewGuid();
            // Act
            var response = await _service.CreateAsync(userId, request);
            await _context.SaveChangesAsync();

            // Assert
            var coverInDb = await _context.CoverLetters.FirstOrDefaultAsync();
            Assert.NotNull(coverInDb);
            Assert.Equal("Test Cover Letter", coverInDb.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSomething_WhenExists()
        {
            // Arrange
            var letterRequest = new CreateCoverLetterRequest(
                "Test Cover Letter",
                "This is a test cover letter.",
                false
            );
            var userId = Guid.NewGuid();

            var letter = new CoverLetter(userId, letterRequest);

            Guid id = letter.Id;

            _context.CoverLetters.Add(letter);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            // ToDo: Testing Ids Assert.Equal(cv.Id, result.Id);
            Assert.Equal(letter.Title, result.Title);
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

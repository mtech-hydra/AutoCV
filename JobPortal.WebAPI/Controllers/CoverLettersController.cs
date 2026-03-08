using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoverLettersController : ControllerBase
{
    private readonly CoverLetterService _service;

    public CoverLettersController(CoverLetterService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync(UserId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(GenerateCoverLetterRequest request)
    {
        var result = await _service.GenerateAIAsync(UserId, request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCoverLetterRequest request)
        => Ok(await _service.CreateAsync(UserId, request));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCoverLetterRequest request)
        => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    private Guid UserId =>
        Guid.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
                   ?? throw new InvalidOperationException("User id claim missing"));
}

public record GenerateCoverLetterRequest(string JobDescription, string CompanyName, string CvSummary, string Skills, string? AiPrompt = null);
public record CreateCoverLetterRequest(string Title, string Content, bool IsAIGenerated, string? AICustomPrompt = null);
public record UpdateCoverLetterRequest(string Title, string Content, string? AICustomPrompt = null);
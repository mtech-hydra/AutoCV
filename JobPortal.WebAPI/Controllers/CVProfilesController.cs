using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CVProfilesController : ControllerBase
{
    private readonly CVService _service;

    public CVProfilesController(CVService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateCVRequest request)
        => Ok(await _service.CreateAsync(UserId, request));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCVRequest request)
        => Ok(await _service.UpdateAsync(id, request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    private Guid UserId => Guid.Parse(User.FindFirst("nameid")!.Value);
}

public record CreateCVRequest(string Title, string Summary, string Skills, string Experience, string Education);
public record UpdateCVRequest(string Title, string Summary, string Skills, string Experience, string Education);
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CVProfilesController : ControllerBase
{
    private readonly CVService _service;

    public CVProfilesController(CVService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Console.WriteLine("[DEBUG] GetAll endpoint called.");

        var result = await _service.GetAllAsync();

        Console.WriteLine($"[DEBUG] GetAll completed. Retrieved {result.Count()} items.");

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        Console.WriteLine($"[DEBUG] Get endpoint called. Id: {id}");

        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            Console.WriteLine($"[DEBUG] No CV found with Id: {id}");
            return NotFound();
        }

        Console.WriteLine($"[DEBUG] Get completed. CV found: {id}");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCVRequest request)
    {
        Console.WriteLine("[DEBUG] Create endpoint called.");
        Console.WriteLine($"[DEBUG] Request data: {System.Text.Json.JsonSerializer.Serialize(request)}");
        Console.WriteLine($"[DEBUG] UserId: {UserId}");

        var result = await _service.CreateAsync(UserId, request);

        Console.WriteLine($"[DEBUG] Create completed. New CV Id: {result.Id}");
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCVRequest request)
    {
        Console.WriteLine($"[DEBUG] Update endpoint called. Id: {id}");
        Console.WriteLine($"[DEBUG] Request: {System.Text.Json.JsonSerializer.Serialize(request)}");

        var result = await _service.UpdateAsync(id, request);

        Console.WriteLine($"[DEBUG] Update completed for Id: {id}");

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    private Guid UserId =>
        Guid.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("UserID Claim is missing"));
}

public record CreateCVRequest(string Title, string Summary, string Skills, string Experience, string Education);
public record UpdateCVRequest(string Title, string Summary, string Skills, string Experience, string Education);
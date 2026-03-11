using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobAdsController : ControllerBase
{
    private readonly JobAdService _service;

    public JobAdsController(JobAdService service) => _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllActiveAsync());

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobAdRequest request)
        => Ok(await _service.CreateAsync(UserId, request));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateJobAdRequest request)
        => Ok(await _service.UpdateAsync(id, request));

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

public record CreateJobAdRequest(string Title, string Description, string CompanyName, string Location, string SalaryRange);
public record CreateJobAdRequestResponse(JobAd JobAd);
public record UpdateJobAdRequest(string Title, string Description, string CompanyName, string Location, string SalaryRange);
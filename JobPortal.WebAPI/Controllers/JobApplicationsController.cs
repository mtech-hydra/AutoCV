using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly JobApplicationService _service;

    public JobApplicationsController(JobApplicationService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync(UserId));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetByIdAsync(UserId, id));

    [HttpPost]
    public async Task<IActionResult> Apply(ApplyJobRequest request)
        => Ok(await _service.ApplyAsync(UserId, request));

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateApplicationStatusRequest request)
        => Ok(await _service.UpdateStatusAsync(UserId, id, request));

    private Guid UserId => Guid.Parse(User.FindFirst("nameid")!.Value);
}

public record ApplyJobRequest(Guid JobAdId, Guid CvProfileId, Guid CoverLetterId);
public record UpdateApplicationStatusRequest(string Status); // Pending, Accepted, Rejected
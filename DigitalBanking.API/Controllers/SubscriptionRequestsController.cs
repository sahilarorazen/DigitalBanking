using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionRequestsController : ControllerBase
{
    private readonly ISubscriptionRequestService _service;

    public SubscriptionRequestsController(
        ISubscriptionRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateSubscriptionRequestDto request,
        CancellationToken cancellationToken)
    {
        var id = await _service.CreateAsync(
            request,
            cancellationToken);

        return Ok(new {
            Id = id
        });
    }

    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> Approve(
        int id,
        ApprovalDecisionDto request,
        CancellationToken cancellationToken)
    {
        await _service.ApproveAsync(
            id,
            request.ApprovedBy,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int}/reject")]
    public async Task<IActionResult> Reject(
        int id,
        ApprovalDecisionDto request,
        CancellationToken cancellationToken)
    {
        await _service.RejectAsync(
            id,
            request.ApprovedBy,
            cancellationToken);

        return NoContent();
    }
}
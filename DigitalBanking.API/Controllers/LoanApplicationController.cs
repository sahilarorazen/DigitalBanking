using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanApplicationController(ILoanApplicationService _loanApplicationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLoanApplication(
        CreateLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var response = await _loanApplicationService.CreateLoanApplicationAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Working Now 3");
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        await _loanApplicationService.HealthAsync(CancellationToken.None);
        return Ok("Database Connected");
    }
}


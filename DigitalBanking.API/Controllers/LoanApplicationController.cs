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
        CreateLoanApplicationRequest createLoanApplicationRequest)
    {
        var response =
            await _loanApplicationService.CreateLoanApplicationAsync(createLoanApplicationRequest);

        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Working");
    }
}


using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[Authorize]
[ApiController]
[Route("api/loan-applications")]
public class LoanApplicationsController(ILoanApplicationService _loanApplicationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLoanApplication(
        CreateLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var response = await _loanApplicationService.CreateLoanApplicationAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result =
            await _loanApplicationService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result =
            await _loanApplicationService.GetByIdAsync(id, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}


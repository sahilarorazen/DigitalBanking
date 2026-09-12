using DigitalBanking.BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/loan-products")]
public class LoanProductsController : ControllerBase
{
    private readonly ILoanProductService _service;

    public LoanProductsController(
        ILoanProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result =
            await _service.GetLoanProductsAsync();

        return Ok(result);
    }

    [HttpGet("{productId}/schemes")]
    public async Task<IActionResult> GetSchemes(
        int productId)
    {
        var result =
            await _service.GetLoanSchemesAsync(productId);

        return Ok(result);
    }
}
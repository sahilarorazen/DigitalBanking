using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AccountReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AccountReadDto>> Create(
        AccountCreateDto request,
        CancellationToken cancellationToken)
    {
        var account = new Account
        {
            AccountNumber = request.AccountNumber,
            CustomerId = request.CustomerId,
            AccountType = request.AccountType,
            Balance = request.Balance
        };

        var createdAccount = await accountService.CreateAsync(account, cancellationToken);
        var response = ToReadDto(createdAccount);
        return CreatedAtAction(nameof(GetById), new { accountId = response.AccountId }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AccountReadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AccountReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var accounts = await accountService.GetAllAsync(cancellationToken);
        return Ok(accounts.Select(ToReadDto).ToList());
    }

    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(AccountReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountReadDto>> GetById(int accountId, CancellationToken cancellationToken)
    {
        var account = await accountService.GetByIdAsync(accountId, cancellationToken);
        return Ok(ToReadDto(account));
    }

    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<AccountReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<AccountReadDto>>> GetByCustomerId(
        int customerId,
        CancellationToken cancellationToken)
    {
        var accounts = await accountService.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(accounts.Select(ToReadDto).ToList());
    }

    [HttpPut("{accountId:int}")]
    [ProducesResponseType(typeof(AccountReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountReadDto>> Update(
        int accountId,
        AccountUpdateDto request,
        CancellationToken cancellationToken)
    {
        var account = new Account
        {
            CustomerId = request.CustomerId,
            AccountType = request.AccountType,
            Balance = request.Balance,
            Status = request.Status
        };

        var updatedAccount = await accountService.UpdateAsync(accountId, account, cancellationToken);
        return Ok(ToReadDto(updatedAccount));
    }

    [HttpDelete("{accountId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int accountId, CancellationToken cancellationToken)
    {
        await accountService.DeleteAsync(accountId, cancellationToken);
        return NoContent();
    }

    private static AccountReadDto ToReadDto(Account account)
    {
        return new AccountReadDto
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            CustomerId = account.CustomerId,
            AccountType = account.AccountType,
            Balance = account.Balance,
            Status = account.Status
        };
    }
}
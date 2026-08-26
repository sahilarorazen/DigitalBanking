using DigitalBanking.BAL.Exceptions;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class AccountService(
    IAccountRepository accountRepository,
    ICustomerRepository customerRepository) : IAccountService
{
    private static readonly string[] AccountTypes = ["Savings", "Current"];
    private static readonly string[] AccountStatuses = ["Open", "Closed", "Blocked"];

    public async Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        ValidateAccount(account, includeStatus: false);
        await EnsureApprovedCustomerAsync(account.CustomerId, cancellationToken);

        account.AccountNumber = account.AccountNumber.Trim();
        account.AccountType = account.AccountType.Trim();
        account.Status = "Open";

        try
        {
            return await accountRepository.AddAsync(account, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new AccountOperationException("The account could not be created.", exception);
        }
    }

    public async Task<Account> GetByIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        ValidateId(accountId);
        var account = await accountRepository.GetByIdAsync(accountId, cancellationToken);
        return account ?? throw new KeyNotFoundException($"Account with ID {accountId} was not found.");
    }

    public Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return accountRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        ValidateId(customerId, "Customer ID");
        await EnsureCustomerExistsAsync(customerId, cancellationToken);
        return await accountRepository.GetByCustomerIdAsync(customerId, cancellationToken);
    }

    public async Task<Account> UpdateAsync(
        int accountId,
        Account account,
        CancellationToken cancellationToken = default)
    {
        ValidateId(accountId);
        ValidateAccount(account, includeStatus: true);

        var existingAccount = await GetByIdAsync(accountId, cancellationToken);
        if (existingAccount.CustomerId != account.CustomerId)
        {
            throw new ArgumentException("An account cannot be transferred to another customer.", nameof(account));
        }

        existingAccount.AccountType = account.AccountType.Trim();
        existingAccount.Balance = account.Balance;
        existingAccount.Status = account.Status.Trim();

        try
        {
            await accountRepository.UpdateAsync(existingAccount, cancellationToken);
            return existingAccount;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new AccountOperationException($"Account with ID {accountId} could not be updated.", exception);
        }
    }

    public async Task DeleteAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var account = await GetByIdAsync(accountId, cancellationToken);

        try
        {
            await accountRepository.DeleteAsync(account, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new AccountOperationException($"Account with ID {accountId} could not be deleted.", exception);
        }
    }

    private async Task EnsureApprovedCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        var customer = await EnsureCustomerExistsAsync(customerId, cancellationToken);
        if (!string.Equals(customer.Status, "Approved", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Only approved customers can create accounts.", nameof(customerId));
        }
    }

    private async Task<Customer> EnsureCustomerExistsAsync(int customerId, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);
        return customer ?? throw new KeyNotFoundException($"Customer with ID {customerId} was not found.");
    }

    private static void ValidateAccount(Account account, bool includeStatus)
    {
        ArgumentNullException.ThrowIfNull(account);
        ValidateId(account.CustomerId, "Customer ID");

        if (string.IsNullOrWhiteSpace(account.AccountNumber))
        {
            throw new ArgumentException("Account number is required.", nameof(account));
        }

        if (!AccountTypes.Contains(account.AccountType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Account type must be Savings or Current.", nameof(account));
        }

        if (account.Balance < 0)
        {
            throw new ArgumentException("Account balance cannot be negative.", nameof(account));
        }

        if (includeStatus && !AccountStatuses.Contains(account.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Account status must be Open, Closed, or Blocked.", nameof(account));
        }
    }

    private static void ValidateId(int id, string name = "Account ID")
    {
        if (id <= 0)
        {
            throw new ArgumentException($"{name} must be greater than zero.", name);
        }
    }
}
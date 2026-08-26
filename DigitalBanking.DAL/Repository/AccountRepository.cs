using DigitalBanking.DAL.Data;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.DAL.Repository;

public class AccountRepository(DigitalBankingDbContext context) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(account => account.AccountId == accountId, cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Accounts.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await context.Accounts.AsNoTracking()
            .Where(account => account.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Account> AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return account;
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Account account, CancellationToken cancellationToken = default)
    {
        context.Accounts.Remove(account);
        await context.SaveChangesAsync(cancellationToken);
    }
}
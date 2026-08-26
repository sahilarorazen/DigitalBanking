using DigitalBanking.DAL.Entities;

namespace DigitalBanking.BAL.Interface;

public interface IAccountService
{
    Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default);

    Task<Account> GetByIdAsync(int accountId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Account>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

    Task<Account> UpdateAsync(int accountId, Account account, CancellationToken cancellationToken = default);

    Task DeleteAsync(int accountId, CancellationToken cancellationToken = default);
}
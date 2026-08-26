using DigitalBanking.DAL.Entities;

namespace DigitalBanking.DAL.Interface;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int accountId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Account>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);

    Task<Account> AddAsync(Account account, CancellationToken cancellationToken = default);

    Task UpdateAsync(Account account, CancellationToken cancellationToken = default);

    Task DeleteAsync(Account account, CancellationToken cancellationToken = default);
}
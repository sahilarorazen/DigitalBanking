using DigitalBanking.DAL.Entities;

namespace DigitalBanking.BAL.Interface;

public interface ICustomerService
{
    Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default);

    Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Customer> UpdateAsync(int id, Customer customer, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
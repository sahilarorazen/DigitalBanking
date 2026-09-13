using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.DAL.Interface;

public interface ILoanApplicationRepository
{
    Task<LoanApplication> CreateAsync(LoanApplication loanApplication, CancellationToken cancellationToken);
    Task<IEnumerable<LoanApplication>> GetAllAsync(CancellationToken cancellationToken);
    Task<LoanApplication?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
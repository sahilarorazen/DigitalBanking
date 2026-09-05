using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.DAL.Interface;

public interface ILoanApplicationRepository
{
    Task<LoanApplication> CreateAsync(LoanApplication loanApplication, CancellationToken cancellationToken);
    Task<LoanApplication> UpdateAsync(LoanApplication loanApplication, CancellationToken cancellationToken);
    Task<LoanApplication?> GetByIdAsync(int loanApplicationId, CancellationToken cancellationToken);
}
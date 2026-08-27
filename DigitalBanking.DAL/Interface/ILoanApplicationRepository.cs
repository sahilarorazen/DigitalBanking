using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.DAL.Interface;

public interface ILoanApplicationRepository
{
    Task<LoanApplication> CreateAsync(LoanApplication loanApplication);
}
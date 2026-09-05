using Microsoft.EntityFrameworkCore;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;
using DigitalBanking.DAL.Data;

namespace DigitalBanking.DAL.Repository;

public class LoanApplicationRepository(DigitalBankingDbContext _digitalBankingDbContext) : ILoanApplicationRepository
{
    public async Task<LoanApplication> CreateAsync(
        LoanApplication loanApplication, CancellationToken cancellationToken)
    {
        _digitalBankingDbContext.LoanApplications.Add(loanApplication);

        await _digitalBankingDbContext.SaveChangesAsync(cancellationToken);

        return loanApplication;
    }

    public async Task<LoanApplication> UpdateAsync(
        LoanApplication loanApplication, CancellationToken cancellationToken)
    {
        _digitalBankingDbContext.LoanApplications.Update(loanApplication);

        await _digitalBankingDbContext.SaveChangesAsync(cancellationToken);

        return loanApplication;
    }

    public async Task<LoanApplication?> GetByIdAsync(
        int loanApplicationId,
        CancellationToken cancellationToken)
    {
        return await _digitalBankingDbContext
            .LoanApplications
            .FirstOrDefaultAsync(
                x => x.Id == loanApplicationId,
                cancellationToken);
    }
}


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
        try
        {
            _digitalBankingDbContext.LoanApplications.Update(loanApplication);

            await _digitalBankingDbContext.SaveChangesAsync(cancellationToken);

            return loanApplication;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.ToString()}");
            Console.WriteLine($"Message: {ex.Message.ToString()}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.ToString()}");
                Console.WriteLine($"Inner Exception Message: {ex.InnerException.Message.ToString()}");
            }
            throw;
        }    
        
    }
    

    public async Task<IEnumerable<LoanApplication>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _digitalBankingDbContext.LoanApplications
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<LoanApplication?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _digitalBankingDbContext.LoanApplications
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}


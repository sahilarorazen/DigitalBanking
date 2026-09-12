using DigitalBanking.DAL.Data;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.DAL.Repository;

public class LoanProductRepository : ILoanProductRepository
{
    private readonly DigitalBankingDbContext _context;

    public LoanProductRepository(DigitalBankingDbContext context)
    {
        _context = context;
    }

    public async Task<List<LoanProduct>> GetLoanProductsAsync()
    {
        return await _context.LoanProducts
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<LoanScheme>> GetLoanSchemesAsync(
        int productId)
    {
        return await _context.LoanSchemes
            .Where(x =>
                x.LoanProductId == productId &&
                x.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }
}
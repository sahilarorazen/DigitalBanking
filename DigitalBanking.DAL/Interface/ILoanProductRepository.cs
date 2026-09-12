using DigitalBanking.DAL.Entities;

namespace DigitalBanking.DAL.Interface;

public interface ILoanProductRepository
{
    Task<List<LoanProduct>> GetLoanProductsAsync();

    Task<List<LoanScheme>> GetLoanSchemesAsync(int productId);
}
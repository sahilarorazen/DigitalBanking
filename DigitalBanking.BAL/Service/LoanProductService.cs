using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class LoanProductService : ILoanProductService
{
    private readonly ILoanProductRepository _repository;

    public LoanProductService(
        ILoanProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LoanProductResponseDto>>
        GetLoanProductsAsync()
    {
        var products =
            await _repository.GetLoanProductsAsync();

        return products.Select(x =>
            new LoanProductResponseDto
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName
            }).ToList();
    }

    public async Task<List<LoanSchemeResponseDto>>
        GetLoanSchemesAsync(int productId)
    {
        var schemes =
            await _repository.GetLoanSchemesAsync(productId);

        return schemes.Select(x =>
            new LoanSchemeResponseDto
            {
                Id = x.Id,
                SchemeCode = x.SchemeCode,
                SchemeName = x.SchemeName,
                InterestRate = x.InterestRate
            }).ToList();
    }
}
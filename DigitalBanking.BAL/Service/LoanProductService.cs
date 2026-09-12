using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class LoanProductService : ILoanProductService
{
    private readonly ILoanProductRepository _repository;
    private readonly ICacheService _cacheService;

    public LoanProductService(
        ILoanProductRepository repository,
        ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<List<LoanProductResponseDto>>
        GetLoanProductsAsync()
    {
        const string cacheKey = "loan-products";

        var cached = await _cacheService.GetAsync<List<LoanProductResponseDto>>(cacheKey);
        
        var products =
            await _repository.GetLoanProductsAsync();

        if (cached != null)
        {
            return cached;
        }

        var result = products.Select(x =>
            new LoanProductResponseDto
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName
            }).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));

        return result;     
    }

    public async Task<List<LoanSchemeResponseDto>>
        GetLoanSchemesAsync(int productId)
    {
        var cacheKey = $"loan-schemes:{productId}";

        var cached = await _cacheService.GetAsync<List<LoanSchemeResponseDto>>(cacheKey);

        if (cached != null)
        {
            return cached;
        }

        var schemes =
            await _repository.GetLoanSchemesAsync(productId);

        var result = schemes.Select(x =>
            new LoanSchemeResponseDto
            {
                Id = x.Id,
                SchemeCode = x.SchemeCode,
                SchemeName = x.SchemeName,
                InterestRate = x.InterestRate
            }).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));

        return result;
    }
}
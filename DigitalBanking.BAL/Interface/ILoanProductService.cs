public interface ILoanProductService
{
    Task<List<LoanProductResponseDto>>
        GetLoanProductsAsync();

    Task<List<LoanSchemeResponseDto>>
        GetLoanSchemesAsync(int productId);
}
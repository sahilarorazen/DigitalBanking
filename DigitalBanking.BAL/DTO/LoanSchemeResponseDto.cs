public class LoanSchemeResponseDto
{
    public int Id { get; set; }

    public string SchemeCode { get; set; } = string.Empty;

    public string SchemeName { get; set; } = string.Empty;

    public decimal InterestRate { get; set; }
}
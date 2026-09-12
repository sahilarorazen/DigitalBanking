namespace DigitalBanking.DAL.Entities;

public class LoanScheme
{
    public int Id { get; set; }

    public int LoanProductId { get; set; }

    public string SchemeCode { get; set; } = string.Empty;

    public string SchemeName { get; set; } = string.Empty;

    public decimal InterestRate { get; set; }

    public bool IsActive { get; set; } = true;

    public LoanProduct LoanProduct { get; set; } = null!;
}
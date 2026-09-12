namespace DigitalBanking.DAL.Entities;

public class LoanProduct
{
    public int Id { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<LoanScheme> LoanSchemes { get; set; }
        = new List<LoanScheme>();
}
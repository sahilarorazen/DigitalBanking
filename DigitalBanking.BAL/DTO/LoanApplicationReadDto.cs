namespace DigitalBanking.BAL.DTO;

public class LoanApplicationReadDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanTermMonths { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
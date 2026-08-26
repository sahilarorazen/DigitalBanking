namespace DigitalBanking.BAL.DTO;

public class AccountReadDto
{
    public int AccountId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string AccountType { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Status { get; set; } = string.Empty;
}
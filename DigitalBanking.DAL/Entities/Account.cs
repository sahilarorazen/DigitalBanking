namespace DigitalBanking.DAL.Entities;

public class Account
{
    public int AccountId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string AccountType { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string Status { get; set; } = "Open";
}
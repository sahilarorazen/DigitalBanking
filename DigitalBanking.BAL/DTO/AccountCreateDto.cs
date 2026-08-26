using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.BAL.DTO;

public class AccountCreateDto
{
    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }

    [Required]
    public string AccountType { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }
}
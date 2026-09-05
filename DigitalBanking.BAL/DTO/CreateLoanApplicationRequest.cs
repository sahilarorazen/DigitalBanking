using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.BAL.DTO;
public class CreateLoanApplicationRequest
{
    [Required]
    public decimal LoanAmount { get; set; }

    [Required]
    public int Tenure { get; set; }

    [Required]
    public decimal InterestRate { get; set; }

    [Required]
    public decimal MonthlyIncome { get; set; }

    public decimal ExistingLiabilities { get; set; }

    [Required]
    public string EmploymentType { get; set; } = string.Empty;
}
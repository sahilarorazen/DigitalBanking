using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.BAL.DTO;
public class CreateLoanApplicationRequest
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int SchemeId { get; set; }

    [Required]
    public string CustomerEmail { get; set; }

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
namespace DigitalBanking.DAL.Entities;

public class LoanApplication
{
    public int Id { get; set; }
    public decimal LoanAmount { get; set; }
    public int Tenure { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal ExistingLiabilities { get; set; }
    public string EmploymentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public decimal RiskScore { get; set; }
    public string Decision { get; set; } = string.Empty;
    public DateTime? AssessmentCompletedDate { get; set; }
}
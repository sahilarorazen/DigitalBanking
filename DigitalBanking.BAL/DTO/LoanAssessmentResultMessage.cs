public class LoanAssessmentResultMessage
{
    public int LoanApplicationId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal RiskScore { get; set; }
    public string Decision { get; set; } = string.Empty;
    public DateTime ProcessedDate { get; set; }
}
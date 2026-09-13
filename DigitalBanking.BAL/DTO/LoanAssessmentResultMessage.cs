public class LoanAssessmentResultMessage
{
    public int LoanApplicationId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerEmail { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal RiskScore { get; set; }
    public string Decision { get; set; } = string.Empty;
    public DateTime ProcessedDate { get; set; }
}
namespace DigitalBanking.LoanAssessmentFunction.Models;

public class LoanAssessmentMessage
{
    public int LoanApplicationId { get; set; }

    public string CustomerId { get; set; }

    public decimal LoanAmount { get; set; }

    public DateTime SubmittedDate { get; set; }
}
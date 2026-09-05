using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.BAL.DTO;

public class CreateLoanApplicationResponse
{
    public int ApplicationId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}
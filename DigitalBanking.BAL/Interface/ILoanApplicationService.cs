using DigitalBanking.BAL.DTO;

namespace DigitalBanking.BAL.Interface;

public interface ILoanApplicationService
{
    Task<CreateLoanApplicationResponse> CreateLoanApplicationAsync(
        CreateLoanApplicationRequest request, CancellationToken cancellationToken);
    Task ProcessLoanAsync(LoanAssessmentMessage message, CancellationToken cancellationToken);
}
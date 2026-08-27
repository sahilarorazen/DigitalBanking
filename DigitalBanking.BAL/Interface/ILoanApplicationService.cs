using DigitalBanking.BAL.DTO;

namespace DigitalBanking.BAL.Interface;

public interface ILoanApplicationService
{
    Task<CreateLoanApplicationResponse> CreateLoanApplicationAsync(
        CreateLoanApplicationRequest createLoanApplicationRequest);
}
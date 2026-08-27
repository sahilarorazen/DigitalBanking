using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class LoanApplicationService(ILoanApplicationRepository _loanApplicationRepository) : ILoanApplicationService
{    
    public async Task<CreateLoanApplicationResponse>
        CreateLoanApplicationAsync(
        CreateLoanApplicationRequest createLoanApplicationRequest)
    {
        var entity = new LoanApplication
        {
            LoanAmount = createLoanApplicationRequest.LoanAmount,
            Tenure = createLoanApplicationRequest.Tenure,
            InterestRate = createLoanApplicationRequest.InterestRate,
            MonthlyIncome = createLoanApplicationRequest.MonthlyIncome,
            ExistingLiabilities = createLoanApplicationRequest.ExistingLiabilities,
            EmploymentType = createLoanApplicationRequest.EmploymentType,
            Status = "Submitted",
            CreatedDate = DateTime.UtcNow
        };

        var result = await _loanApplicationRepository.CreateAsync(entity);

        return new CreateLoanApplicationResponse
        {
            ApplicationId = result.Id,
            Status = result.Status,
            CreatedDate = result.CreatedDate
        };
    }
}
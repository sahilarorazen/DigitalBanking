using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class LoanApplicationService(ILoanApplicationRepository _loanApplicationRepository) : ILoanApplicationService
{    
    public async Task<CreateLoanApplicationResponse>
        CreateLoanApplicationAsync(
        CreateLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var entityLoanApplication = new LoanApplication
        {
            LoanAmount = request.LoanAmount,
            Tenure = request.Tenure,
            InterestRate = request.InterestRate,
            MonthlyIncome = request.MonthlyIncome,
            ExistingLiabilities = request.ExistingLiabilities,
            EmploymentType = request.EmploymentType,
            Status = "Submitted",
            CreatedDate = DateTime.UtcNow
        };

        var result = await _loanApplicationRepository.CreateAsync(entityLoanApplication, cancellationToken);

        return new CreateLoanApplicationResponse
        {
            ApplicationId = result.Id,
            Status = result.Status,
            CreatedDate = result.CreatedDate
        };
    }

    public async Task HealthAsync(CancellationToken cancellationToken)
    {
        await _loanApplicationRepository.HealthAsync(cancellationToken);
    }
}
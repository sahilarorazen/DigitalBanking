using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class LoanApplicationService(
ILoanApplicationRepository _loanApplicationRepository, 
IServiceBusPublisherService _serviceBusPublisher,
IAssessmentResultPublisherService _assessmentResultPublisher) : ILoanApplicationService
{    
    public async Task<CreateLoanApplicationResponse>
        CreateLoanApplicationAsync(
        CreateLoanApplicationRequest request, CancellationToken cancellationToken)
    {
        var entityLoanApplication = new LoanApplication
        {
            CustomerId = request.CustomerId,
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

        var message =
            new LoanAssessmentMessage
            {
                LoanApplicationId = result.Id,
                CustomerId = result.CustomerId.ToString(),
                LoanAmount = result.LoanAmount,
                SubmittedDate = DateTime.UtcNow
            };

        await _serviceBusPublisher.PublishAsync(message);

        return new CreateLoanApplicationResponse
        {
            ApplicationId = result.Id,
            Status = result.Status,
            CreatedDate = result.CreatedDate
        };
    }

    public async Task ProcessLoanAsync(LoanAssessmentMessage message, CancellationToken cancellationToken)
    {
        // Fetch loan application
        var loanApplication = await 
        _loanApplicationRepository.GetByIdAsync(message.LoanApplicationId, cancellationToken);

        if (loanApplication == null)
        {
            throw new Exception(
                $"Loan Application {message.LoanApplicationId} not found");
        }

        // Step 1 - Assessment Started
        loanApplication.Status = "Assessment In Progress";

        // Step 2 - Eligibility Check
        bool isEligible =
            loanApplication.MonthlyIncome >= 25000
            && loanApplication.EmploymentType != "Unemployed";

        decimal riskScore = 100;

        // Step 3 - Risk Score Calculation
        decimal debtToIncomeRatio =
            loanApplication.ExistingLiabilities /
            loanApplication.MonthlyIncome;

        if (debtToIncomeRatio > 0.60m)
        {
            riskScore -= 40;
        }
        else if (debtToIncomeRatio > 0.40m)
        {
            riskScore -= 20;
        }

        if (loanApplication.EmploymentType == "Self Employed")
        {
            riskScore -= 10;
        }

        if (loanApplication.LoanAmount > 1000000)
        {
            riskScore -= 10;
        }

        // Step 4 - Decision Engine
        string decision;

        if (!isEligible)
        {
            decision = "Rejected";
        }
        else if (riskScore >= 70)
        {
            decision = "Approved";
        }
        else if (riskScore >= 50)
        {
            decision = "Manual Review";
        }
        else
        {
            decision = "Rejected";
        }

        // Step 5 - Final Update
        loanApplication.Status = decision;

        loanApplication.RiskScore = riskScore;
        loanApplication.Decision = decision;
        loanApplication.AssessmentCompletedDate = DateTime.UtcNow;

        await _loanApplicationRepository.CreateAsync(loanApplication, cancellationToken); 

        var resultMessage =
        new LoanAssessmentResultMessage
        {
            LoanApplicationId = loanApplication.Id,
            CustomerId = loanApplication.CustomerId.ToString(),
            LoanAmount = loanApplication.LoanAmount,
            RiskScore = riskScore,
            Decision = decision,
            ProcessedDate = DateTime.UtcNow
        };

        await _assessmentResultPublisher.PublishAsync(
            resultMessage,
            cancellationToken);           
    }

    public async Task<IEnumerable<LoanApplicationReadDto>> GetAllAsync()
    {
        var loans = await _loanApplicationRepository.GetAllAsync();

        return loans.Select(x => new LoanApplicationReadDto
        {
            Id = x.Id,
            CustomerId = x.CustomerId,
            LoanAmount = x.LoanAmount,
            LoanTermMonths = x.Tenure,
            Status = x.Status,
            CreatedDate = x.CreatedDate
        });
    }

    public async Task<LoanApplicationReadDto?> GetByIdAsync(int id)
    {
        var loan = await _loanApplicationRepository.GetByIdAsync(id, CancellationToken.None);

        if (loan == null)
            return null;

        return new LoanApplicationReadDto
        {
            Id = loan.Id,
            CustomerId = loan.CustomerId,
            LoanAmount = loan.LoanAmount,
            LoanTermMonths = loan.Tenure,
            Status = loan.Status,
            CreatedDate = loan.CreatedDate
        };
    }
}



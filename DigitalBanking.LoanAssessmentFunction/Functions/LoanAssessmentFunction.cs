using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DigitalBanking.BAL.Interface;
using DigitalBanking.BAL.DTO;

namespace DigitalBanking.LoanAssessmentFunction.Functions;

public class LoanAssessmentFunction
{
    private readonly ILogger<LoanAssessmentFunction> _logger;
    private readonly ILoanApplicationService _service;

    public LoanAssessmentFunction(
        ILogger<LoanAssessmentFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(LoanAssessmentFunction))]
    public async Task Run(
        [ServiceBusTrigger(
            "loan-assessment-queue",
            Connection = "ServiceBusConnection")]
        string message)
    {
        var assessmentMessage = System.Text.Json.JsonSerializer.Deserialize<LoanAssessmentMessage>(message);
        
        if (assessmentMessage == null)
        {
            _logger.LogError("Failed to deserialize message: {message}", message);
            return;
        }
        
        await _service.ProcessLoanAsync(assessmentMessage, CancellationToken.None);
    }
}
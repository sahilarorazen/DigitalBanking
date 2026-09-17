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
        ILoanApplicationService service,
        ILogger<LoanAssessmentFunction> logger
        )
    {
        _logger = logger;
        _service = service;
    }

    [Function(nameof(LoanAssessmentFunction))]
    public async Task Run(
        [ServiceBusTrigger(
            "loan-assessment-queue",
            Connection = "ServiceBusConnection")]
        string message)
    {
         _logger.LogInformation("LoanAssessmentFunction Triggered");
        
        var assessmentMessage = System.Text.Json.JsonSerializer.Deserialize<LoanAssessmentMessage>(message);
        
        if (assessmentMessage == null)
        {
            _logger.LogError("Failed to deserialize message: {message}", message);
            return;
        }
        
        await _service.ProcessLoanAsync(assessmentMessage, CancellationToken.None);
    }
}
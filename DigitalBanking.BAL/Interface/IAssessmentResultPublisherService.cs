public interface IAssessmentResultPublisherService
{
    Task PublishAsync(LoanAssessmentResultMessage message, CancellationToken cancellationToken);
}
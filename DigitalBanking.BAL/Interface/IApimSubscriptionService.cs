using DigitalBanking.BAL.DTO;

public interface IApimSubscriptionService
{
    Task<ApimSubscriptionResponseDto>
        CreateSubscriptionAsync(
            SubscriptionRequest request,
            CancellationToken cancellationToken);
}
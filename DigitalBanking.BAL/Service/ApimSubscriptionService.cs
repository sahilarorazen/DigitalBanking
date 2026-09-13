using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;

namespace DigitalBanking.BAL.Service;

public class ApimSubscriptionService
    : IApimSubscriptionService
{
    public Task<ApimSubscriptionResponseDto>
        CreateSubscriptionAsync(
            SubscriptionRequest request,
            CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new ApimSubscriptionResponseDto
            {
                SubscriptionId =
                    Guid.NewGuid().ToString(),

                PrimaryKey =
                    Guid.NewGuid().ToString("N"),

                SecondaryKey =
                    Guid.NewGuid().ToString("N")
            });
    }
}
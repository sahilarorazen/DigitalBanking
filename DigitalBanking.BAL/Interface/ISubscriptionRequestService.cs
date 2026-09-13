public interface ISubscriptionRequestService
{
    Task<int> CreateAsync(
        CreateSubscriptionRequestDto dto, CancellationToken cancellationToken);

    Task ApproveAsync(
        int requestId,
        string approvedBy, CancellationToken cancellationToken);

    Task RejectAsync(
        int requestId,
        string approvedBy, CancellationToken cancellationToken);
}
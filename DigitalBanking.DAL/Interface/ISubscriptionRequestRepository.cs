public interface ISubscriptionRequestRepository
{
    Task<SubscriptionRequest> AddAsync(SubscriptionRequest request, CancellationToken cancellationToken = default);

    Task<SubscriptionRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task UpdateAsync(SubscriptionRequest request, CancellationToken cancellationToken = default);
}
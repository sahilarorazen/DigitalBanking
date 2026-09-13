namespace DigitalBanking.BAL.Interface;

public interface IServiceBusPublisherService
{
    Task PublishAsync<T>(
        string entityName,
        T message,
        CancellationToken cancellationToken = default);
}
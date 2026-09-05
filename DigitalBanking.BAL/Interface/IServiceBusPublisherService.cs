namespace DigitalBanking.BAL.Interface;

public interface IServiceBusPublisherService
{
    Task PublishAsync<T>(T message);
}
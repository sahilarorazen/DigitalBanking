using Azure.Identity;
using Azure.Messaging.ServiceBus;
using DigitalBanking.BAL.Interface;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DigitalBanking.BAL.Service;

public class ServiceBusPublisherService : IServiceBusPublisherService
{
    private readonly ServiceBusClient _client;

    public ServiceBusPublisherService(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishAsync<T>(
        string entityName,
        T message,
        CancellationToken cancellationToken = default)
    {
        await using var sender =
            _client.CreateSender(entityName);

        var json =
            JsonSerializer.Serialize(message);

        await sender.SendMessageAsync(
            new ServiceBusMessage(json)
            {
                Subject = typeof(T).Name
            },
            cancellationToken);
    }
}
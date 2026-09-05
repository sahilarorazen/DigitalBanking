using Azure.Identity;
using Azure.Messaging.ServiceBus;
using DigitalBanking.BAL.Interface;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DigitalBanking.BAL.Service;

public class ServiceBusPublisherService : IServiceBusPublisherService
{
    private readonly ServiceBusSender _sender;

    public ServiceBusPublisherService(IConfiguration configuration)
    {
        var namespaceName =
            configuration["ServiceBus:Namespace"];

        var queueName =
            configuration["ServiceBus:QueueName"];

        var clientId =
            configuration["ManagedIdentityClientId"];

        var credential =
            new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = clientId
                });

        var client =
            new ServiceBusClient(
                namespaceName,
                credential);

        _sender = client.CreateSender(queueName);
    }

    public async Task PublishAsync<T>(T message)
    {
        var json =
            JsonSerializer.Serialize(message);

        await _sender.SendMessageAsync(
            new ServiceBusMessage(json));
    }
}
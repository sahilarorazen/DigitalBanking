using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

public class AssessmentResultPublisherService
    : IAssessmentResultPublisherService
{
    private readonly IConfiguration _configuration;

    public AssessmentResultPublisherService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync(
        LoanAssessmentResultMessage message,
        CancellationToken cancellationToken)
    {
        string namespaceName =
            _configuration["ServiceBus:Namespace"];

        string topicName =
            _configuration["ServiceBus:AssessmentTopic"];

        var client = new ServiceBusClient(
            namespaceName,
            new DefaultAzureCredential());

        var sender = client.CreateSender(topicName);

        string json =
            JsonSerializer.Serialize(message);

        await sender.SendMessageAsync(
            new ServiceBusMessage(json),
            cancellationToken);

        await sender.DisposeAsync();
        await client.DisposeAsync();
    }
}
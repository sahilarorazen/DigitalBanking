using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Core;
using Azure.Identity;
using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using Microsoft.Extensions.Configuration;
namespace DigitalBanking.BAL.Service;

public class ApimSubscriptionService
    : IApimSubscriptionService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ApimSubscriptionService(
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<ApimSubscriptionResponseDto>
        CreateSubscriptionAsync(
            SubscriptionRequest request,
            CancellationToken cancellationToken)
    {
        var credential =
            new DefaultAzureCredential();

        var token =
            await credential.GetTokenAsync(
                new TokenRequestContext(
                    new[]
                    {
                        "https://management.azure.com/.default"
                    }),
                cancellationToken);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token.Token);

        var subscriptionId =
            _configuration["Apim:SubscriptionId"];

        var resourceGroup =
            _configuration["Apim:ResourceGroup"];

        var serviceName =
            _configuration["Apim:ServiceName"];

        var apimSubscriptionName =
            Guid.NewGuid().ToString();

        var productId =
            request.ProductId;

        var url =
            $"https://management.azure.com/subscriptions/{subscriptionId}" +
            $"/resourceGroups/{resourceGroup}" +
            $"/providers/Microsoft.ApiManagement/service/{serviceName}" +
            $"/subscriptions/{apimSubscriptionName}" +
            $"?api-version=2024-05-01";

        var payload = new
        {
            properties = new
            {
                displayName =
                    request.ApplicationName,

                scope =
                    $"/products/{productId}",

                state = "active"
            }
        };

        var json =
            JsonSerializer.Serialize(payload);

        var response =
            await _httpClient.PutAsync(
                url,
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"),
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseContent =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        using var document =
            JsonDocument.Parse(responseContent);

        return new ApimSubscriptionResponseDto
        {
            SubscriptionId =
                document.RootElement
                    .GetProperty("name")
                    .GetString() ?? "",

            PrimaryKey =
                document.RootElement
                    .GetProperty("properties")
                    .GetProperty("primaryKey")
                    .GetString() ?? "",

            SecondaryKey =
                document.RootElement
                    .GetProperty("properties")
                    .GetProperty("secondaryKey")
                    .GetString() ?? ""
        };
    }
}
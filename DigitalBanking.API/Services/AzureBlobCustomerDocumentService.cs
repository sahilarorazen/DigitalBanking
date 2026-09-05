using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;

namespace DigitalBanking.API.Services;

public sealed class AzureBlobCustomerDocumentService : ICustomerDocumentService
{
    private readonly BlobContainerClient containerClient;

    public AzureBlobCustomerDocumentService(IConfiguration configuration)
    {
        var accountName =
        configuration["BlobStorage:AccountName"]
        ?? throw new InvalidOperationException("BlobStorage:AccountName missing");

        var containerName =
        configuration["BlobStorage:ContainerName"]
        ?? "customer-documents";

        var  clientId =
        configuration["ManagedIdentityClientId"]
        ?? throw new InvalidOperationException("ManagedIdentityClientId missing");

        var credential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = clientId
            });

        var blobUri = new Uri(
        $"https://{accountName}.blob.core.windows.net/{containerName}");

        containerClient = new BlobContainerClient(
        blobUri,
        credential);
    }

    public async Task<CustomerDocument> UploadAsync(
        int customerId,
        IFormFile document,
        CancellationToken cancellationToken = default)
    {
        ValidateCustomerId(customerId);
        ArgumentNullException.ThrowIfNull(document);

        if (document.Length == 0)
        {
            throw new ArgumentException("The document cannot be empty.", nameof(document));
        }

        var fileName = Path.GetFileName(document.FileName);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("A document file name is required.", nameof(document));
        }

        var documentId = Guid.NewGuid();
        var blobName = GetBlobName(customerId, documentId);
        var blobClient = containerClient.GetBlobClient(blobName);
        var contentType = string.IsNullOrWhiteSpace(document.ContentType)
            ? "application/octet-stream"
            : document.ContentType;

        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        await using var content = document.OpenReadStream();
        await blobClient.UploadAsync(
            content,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                },
                Metadata = new Dictionary<string, string>
                {
                    ["fileName"] = fileName
                }
            },
            cancellationToken);

        return new CustomerDocument(
            documentId,
            customerId,
            fileName,
            contentType,
            document.Length);
    }

    public async Task<CustomerDocumentDownload?> DownloadAsync(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        ValidateCustomerId(customerId);
        ValidateDocumentId(documentId);

        var blobClient = containerClient.GetBlobClient(GetBlobName(customerId, documentId));

        try
        {
            var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
            var properties = response.Value.Details;
            var fileName = properties.Metadata.TryGetValue("fileName", out var storedFileName)
                ? storedFileName
                : $"{documentId}.bin";

            return new CustomerDocumentDownload(
                response.Value.Content,
                fileName,
                properties.ContentType ?? "application/octet-stream");
        }
        catch (RequestFailedException exception) when (exception.Status == 404)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        ValidateCustomerId(customerId);
        ValidateDocumentId(documentId);

        var blobClient = containerClient.GetBlobClient(GetBlobName(customerId, documentId));
        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        return response.Value;
    }

    private static string GetBlobName(int customerId, Guid documentId) =>
        $"{customerId}/{documentId:N}";

    private static void ValidateCustomerId(int customerId)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));
        }
    }

    private static void ValidateDocumentId(Guid documentId)
    {
        if (documentId == Guid.Empty)
        {
            throw new ArgumentException("Document ID is required.", nameof(documentId));
        }
    }
}
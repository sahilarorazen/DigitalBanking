using Microsoft.AspNetCore.Http;

namespace DigitalBanking.API.Services;

public interface ICustomerDocumentService
{
    Task<CustomerDocument> UploadAsync(
        int customerId,
        IFormFile document,
        CancellationToken cancellationToken = default);

    Task<CustomerDocumentDownload?> DownloadAsync(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken = default);
}

public sealed record CustomerDocument(
    Guid DocumentId,
    int CustomerId,
    string FileName,
    string ContentType,
    long Length);

public sealed record CustomerDocumentDownload(
    Stream Content,
    string FileName,
    string ContentType);
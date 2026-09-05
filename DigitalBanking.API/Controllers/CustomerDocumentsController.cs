using DigitalBanking.API.Services;
using DigitalBanking.BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/customers/{customerId:int}/documents")]
public class CustomerDocumentsController(
    ICustomerService customerService,
    ICustomerDocumentService documentService) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CustomerDocument), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDocument>> Upload(
        int customerId,
        IFormFile document,
        CancellationToken cancellationToken)
    {
        await customerService.GetByIdAsync(customerId, cancellationToken);
        var uploadedDocument = await documentService.UploadAsync(customerId, document, cancellationToken);

        return CreatedAtAction(
            nameof(Download),
            new { customerId, documentId = uploadedDocument.DocumentId },
            uploadedDocument);
    }

    [HttpGet("{documentId:guid}")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken)
    {
        var document = await documentService.DownloadAsync(customerId, documentId, cancellationToken);
        return document is null
            ? NotFound()
            : File(document.Content, document.ContentType, document.FileName);
    }

    [HttpDelete("{documentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int customerId,
        Guid documentId,
        CancellationToken cancellationToken)
    {
        var deleted = await documentService.DeleteAsync(customerId, documentId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
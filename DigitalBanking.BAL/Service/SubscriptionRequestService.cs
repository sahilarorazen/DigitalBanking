using DigitalBanking.BAL.Constants;
using DigitalBanking.BAL.Interface;
using Microsoft.Extensions.Configuration;

namespace DigitalBanking.Application.Services;

public class SubscriptionRequestService : ISubscriptionRequestService
{
    private readonly ISubscriptionRequestRepository _repository;
    private readonly IServiceBusPublisherService _publisher;
    private readonly IConfiguration _configuration;
    private readonly IApimSubscriptionService _apimSubscriptionService;
    
    public SubscriptionRequestService(
        ISubscriptionRequestRepository repository,
        IServiceBusPublisherService publisher,
        IConfiguration configuration, IApimSubscriptionService apimSubscriptionService)
    {
        _repository = repository;
        _publisher = publisher;
        _configuration = configuration;
        _apimSubscriptionService = apimSubscriptionService;
    }

    public async Task<int> CreateAsync(
        CreateSubscriptionRequestDto dto, CancellationToken cancellationToken)
    {
        var request = new SubscriptionRequest
        {
            ProductId = dto.ProductId,
            ProductName = dto.ProductName,
            ApplicationName = dto.ApplicationName,
            BusinessOwner = dto.BusinessOwner,
            Email = dto.Email,
            Justification = dto.Justification,
            Status = SubscriptionRequestStatus.Pending.ToString(),
            RequestedDate = DateTime.UtcNow
        };

        request = await _repository.AddAsync(request, cancellationToken);

        await _publisher.PublishAsync(_configuration["ServiceBus:SubscriptionRequestsTopic"],
            new SubscriptionRequestMessage
            {
                RequestId = request.Id,
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                ApplicationName = request.ApplicationName,
                BusinessOwner = request.BusinessOwner,
                Email = request.Email
            }, cancellationToken);

        return request.Id;
    }

    public async Task ApproveAsync(
        int requestId,
        string approvedBy, CancellationToken cancellationToken)
    {
        var request =
            await _repository.GetByIdAsync(requestId, cancellationToken);

        if (request is null)
        {
            throw new KeyNotFoundException(
                $"Subscription Request {requestId} not found.");
        }

        if (request.Status != SubscriptionRequestStatus.Pending.ToString())
        {
            throw new InvalidOperationException(
                $"Request {requestId} has already been processed.");
        }

        var apimSubscription = await _apimSubscriptionService
                                .CreateSubscriptionAsync(request, cancellationToken);
        request.Status = SubscriptionRequestStatus.Approved.ToString();
        request.ApprovedBy = approvedBy;
        request.ApprovedDate = DateTime.UtcNow;
        request.SubscriptionId = apimSubscription.SubscriptionId;
        request.PrimaryKey = apimSubscription.PrimaryKey;
        request.SecondaryKey = apimSubscription.SecondaryKey;
        await _repository.UpdateAsync(request, cancellationToken);
    }

    public async Task RejectAsync(
        int requestId,
        string approvedBy, CancellationToken cancellationToken)
    {
        var request =
            await _repository.GetByIdAsync(requestId, cancellationToken);

        if (request is null)
        {
            throw new KeyNotFoundException(
                $"Subscription Request {requestId} not found.");
        }

        if (request.Status != SubscriptionRequestStatus.Pending.ToString())
        {
            throw new InvalidOperationException(
                $"Request {requestId} has already been processed.");
        }

        request.Status = SubscriptionRequestStatus.Rejected.ToString();
        request.ApprovedBy = approvedBy;
        request.ApprovedDate = DateTime.UtcNow;

        await _repository.UpdateAsync(request, cancellationToken);
    }
}
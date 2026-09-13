using DigitalBanking.DAL.Data;
using Microsoft.EntityFrameworkCore;

public class SubscriptionRequestRepository : ISubscriptionRequestRepository
{
    private readonly DigitalBankingDbContext _context;

    public SubscriptionRequestRepository(
        DigitalBankingDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionRequest> AddAsync(
        SubscriptionRequest request, CancellationToken cancellationToken)
    {
        _context.SubscriptionRequests.Add(request);

        await _context.SaveChangesAsync(cancellationToken);

        return request;
    }

    public async Task<SubscriptionRequest?> GetByIdAsync(
        int id, CancellationToken cancellationToken)
    {
        return await _context.SubscriptionRequests
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(
        SubscriptionRequest request, CancellationToken cancellationToken)
    {
        _context.SubscriptionRequests.Update(request);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
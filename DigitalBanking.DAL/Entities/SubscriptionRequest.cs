public class SubscriptionRequest
{
    public int Id { get; set; }

    public string ProductId { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string ApplicationName { get; set; } = string.Empty;

    public string BusinessOwner { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Justification { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTime RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovedBy { get; set; }

    public string? SubscriptionId { get; set; }
}
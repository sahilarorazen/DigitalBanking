public class SubscriptionRequestMessage
{
    public int RequestId { get; set; }

    public string ProductId { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string ApplicationName { get; set; } = string.Empty;

    public string BusinessOwner { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
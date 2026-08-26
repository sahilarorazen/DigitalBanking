namespace DigitalBanking.DAL.Entities;

public class Customer
{
    public int Id { get; set; }

    public string CustomerId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string PanId { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string EmploymentDetails { get; set; } = string.Empty;

    public decimal IncomeDetails { get; set; }

    public string Status { get; set; } = "Submitted";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }
}
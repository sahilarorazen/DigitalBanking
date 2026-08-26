using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.BAL.DTO;

public class CustomerCreateDto
{
    [Required]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string MobileNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string EmploymentDetails { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal IncomeDetails { get; set; }
}
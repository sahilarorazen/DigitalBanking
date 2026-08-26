using System.Net.Mail;
using DigitalBanking.BAL.Exceptions;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using DigitalBanking.DAL.Interface;

namespace DigitalBanking.BAL.Service;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        ValidateCustomer(customer);

        customer.Status = string.IsNullOrWhiteSpace(customer.Status) ? "Submitted" : customer.Status.Trim();
        customer.CreatedDate = customer.CreatedDate == default ? DateTime.UtcNow : customer.CreatedDate;

        try
        {
            return await customerRepository.AddAsync(customer, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new CustomerOperationException("The customer could not be created.", exception);
        }
    }

    public async Task<Customer> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        ValidateId(id);

        var customer = await customerRepository.GetByIdAsync(id, cancellationToken);
        return customer ?? throw new KeyNotFoundException($"Customer with ID {id} was not found.");
    }

    public Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return customerRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Customer> UpdateAsync(int id, Customer customer, CancellationToken cancellationToken = default)
    {
        ValidateId(id);
        ValidateCustomer(customer);

        var existingCustomer = await GetByIdAsync(id, cancellationToken);

        existingCustomer.CustomerId = customer.CustomerId.Trim();
        existingCustomer.Name = customer.Name.Trim();
        existingCustomer.DateOfBirth = customer.DateOfBirth;
        existingCustomer.PanId = customer.PanId.Trim();
        existingCustomer.MobileNumber = customer.MobileNumber.Trim();
        existingCustomer.EmailAddress = customer.EmailAddress.Trim();
        existingCustomer.Address = customer.Address.Trim();
        existingCustomer.EmploymentDetails = customer.EmploymentDetails.Trim();
        existingCustomer.IncomeDetails = customer.IncomeDetails;
        existingCustomer.Status = string.IsNullOrWhiteSpace(customer.Status)
            ? existingCustomer.Status
            : customer.Status.Trim();
        existingCustomer.ModifiedDate = DateTime.UtcNow;

        try
        {
            await customerRepository.UpdateAsync(existingCustomer, cancellationToken);
            return existingCustomer;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new CustomerOperationException($"Customer with ID {id} could not be updated.", exception);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        ValidateId(id);
        var customer = await GetByIdAsync(id, cancellationToken);

        try
        {
            await customerRepository.DeleteAsync(customer, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new CustomerOperationException($"Customer with ID {id} could not be deleted.", exception);
        }
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }
    }

    private static void ValidateCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        if (string.IsNullOrWhiteSpace(customer.CustomerId))
        {
            throw new ArgumentException("Customer ID is required.", nameof(customer));
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            throw new ArgumentException("Customer name is required.", nameof(customer));
        }

        if (customer.DateOfBirth == default || customer.DateOfBirth > DateTime.UtcNow)
        {
            throw new ArgumentException("Date of birth must be a valid date that is not in the future.", nameof(customer));
        }

        if (string.IsNullOrWhiteSpace(customer.EmailAddress) || !IsValidEmail(customer.EmailAddress))
        {
            throw new ArgumentException("A valid email address is required.", nameof(customer));
        }

        if (customer.IncomeDetails < 0)
        {
            throw new ArgumentException("Income details cannot be negative.", nameof(customer));
        }
    }

    private static bool IsValidEmail(string emailAddress)
    {
        try
        {
            var mailAddress = new MailAddress(emailAddress.Trim());
            return mailAddress.Address.Equals(emailAddress.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
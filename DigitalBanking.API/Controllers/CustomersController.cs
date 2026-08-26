using DigitalBanking.BAL.DTO;
using DigitalBanking.BAL.Interface;
using DigitalBanking.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CustomerReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerReadDto>> Register(
        CustomerCreateDto request,
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerId = request.CustomerId,
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            MobileNumber = request.MobileNumber,
            EmailAddress = request.EmailAddress,
            Address = request.Address,
            EmploymentDetails = request.EmploymentDetails,
            IncomeDetails = request.IncomeDetails
        };

        var createdCustomer = await customerService.CreateAsync(customer, cancellationToken);
        var response = ToReadDto(createdCustomer);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerReadDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await customerService.GetByIdAsync(id, cancellationToken);
        return Ok(ToReadDto(customer));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerReadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CustomerReadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await customerService.GetAllAsync(cancellationToken);
        return Ok(customers.Select(ToReadDto).ToList());
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CustomerReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerReadDto>> Update(
        int id,
        CustomerUpdateDto request,
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            CustomerId = request.CustomerId,
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            MobileNumber = request.MobileNumber,
            EmailAddress = request.EmailAddress,
            Address = request.Address,
            EmploymentDetails = request.EmploymentDetails,
            IncomeDetails = request.IncomeDetails,
            Status = request.Status ?? string.Empty
        };

        var updatedCustomer = await customerService.UpdateAsync(id, customer, cancellationToken);
        return Ok(ToReadDto(updatedCustomer));
    }

    private static CustomerReadDto ToReadDto(Customer customer)
    {
        return new CustomerReadDto
        {
            Id = customer.Id,
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            DateOfBirth = customer.DateOfBirth,
            MobileNumber = customer.MobileNumber,
            EmailAddress = customer.EmailAddress,
            Address = customer.Address,
            EmploymentDetails = customer.EmploymentDetails,
            IncomeDetails = customer.IncomeDetails,
            Status = customer.Status,
            CreatedDate = customer.CreatedDate,
            ModifiedDate = customer.ModifiedDate
        };
    }
}
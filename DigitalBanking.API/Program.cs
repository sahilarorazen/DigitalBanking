using DigitalBanking.API;
using DigitalBanking.BAL.Interface;
using DigitalBanking.BAL.Service;
using DigitalBanking.DAL.Data;
using DigitalBanking.DAL.Interface;
using DigitalBanking.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Core;
using Microsoft.Data.SqlClient;
using DigitalBanking.BAL.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHealthChecks()
.AddDbContextCheck<DigitalBankingDbContext>(
name: "database");

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerDocumentService, CustomerDocumentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Banking API v1");
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString()
            })
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(result));
    }
});
app.Run();

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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<DigitalBankingDbContext>((sp, options) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionString =
        configuration.GetConnectionString("DefaultConnection");

    var clientId =
        configuration["ManagedIdentity:ClientId"];

    var credential = new DefaultAzureCredential(
        new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = clientId
        });

    var connection = new SqlConnection(connectionString);

    var token =
        credential.GetToken(
            new Azure.Core.TokenRequestContext(
                new[]
                {
                    "https://database.windows.net/.default"
                }));

    connection.AccessToken = token.Token;

    options.UseSqlServer(connection);
});
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
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

app.Run();

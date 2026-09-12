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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    var connection = new SqlConnection(connectionString);

    // var credential = new AzureCliCredential();
    var credential = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = builder.Configuration["ManagedIdentityClientId"]
                });

    var token = credential.GetToken(
        new TokenRequestContext(
            new[] { "https://database.windows.net/.default" }));

    connection.AccessToken = token.Token;

    options.UseSqlServer(connection);
});
// builder.Services.AddHealthChecks();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAssessmentResultPublisherService, AssessmentResultPublisherService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
builder.Services.AddScoped<IServiceBusPublisherService,ServiceBusPublisherService>();
builder.Services.AddSingleton<ICustomerDocumentService, CustomerDocumentService>();
builder.Services.AddScoped<ILoanProductService,LoanProductService>();

builder.Services.AddScoped<ILoanProductRepository,LoanProductRepository>();
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

app.MapGet("/dbtest", async (DigitalBankingDbContext db) =>
{
    try
    {
        await db.Database.CanConnectAsync();
        return Results.Ok("Connected");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
});

app.MapGet("/customers-test", async () =>
{
    try
    {
        // var credential = new AzureCliCredential();
        var credential = new DefaultAzureCredential();

        var token = await credential.GetTokenAsync(
            new TokenRequestContext(
                new[] { "https://database.windows.net/.default" }));

        return Results.Ok("Token OK");
    }
    catch (Exception ex)
    {
        return Results.Ok(ex.ToString());
    }
});

app.MapGet("/conn", (IConfiguration config) =>
{
    return config.GetConnectionString("DefaultConnection");
});

app.MapGet("/token-test", async () =>
{
    try
    {
        // var credential = new AzureCliCredential();
        var credential = new DefaultAzureCredential();

        var token = await credential.GetTokenAsync(
            new TokenRequestContext(
                new[] { "https://database.windows.net/.default" }));

        return Results.Ok(token.Token.Substring(0,20));
    }
    catch (Exception ex)
    {
        return Results.Ok(ex.ToString());
    }
});

app.Run();

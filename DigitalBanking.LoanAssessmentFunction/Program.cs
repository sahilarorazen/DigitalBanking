// using Azure.Monitor.OpenTelemetry.Exporter;
using DigitalBanking.DAL.Data;
using DigitalBanking.DAL.Interface;
using DigitalBanking.DAL.Repository;
using DigitalBanking.BAL.Interface;
using DigitalBanking.BAL.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
// using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using OpenTelemetry;
using Microsoft.Data.SqlClient;
using Azure.Identity;
using Azure.Core;
using DigitalBanking.Application.Services;
using DigitalBanking.BAL.Services;
using Azure.Messaging.ServiceBus;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(sp =>
{
    var connectionString =
        builder.Configuration["ServiceBusConnection"];

    return new ServiceBusClient(connectionString);
});

// builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
// {
//     var connectionString =
//         builder.Configuration.GetConnectionString("DefaultConnection");

//     var connection = new SqlConnection(connectionString);

//     // var credential = new AzureCliCredential();
//     var credential = new DefaultAzureCredential(
//         new DefaultAzureCredentialOptions
//         {
//             ManagedIdentityClientId = builder.Configuration["ManagedIdentityClientId"]
//         });

//     var token = credential.GetToken(
//         new TokenRequestContext(
//             new[] { "https://database.windows.net/.default" }));

//     connection.AccessToken = token.Token;

//     options.UseSqlServer(connection);
// });

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
builder.Services.AddScoped<IServiceBusPublisherService,ServiceBusPublisherService>();
builder.Services.AddSingleton<ICustomerDocumentService, CustomerDocumentService>();
builder.Services.AddScoped<ILoanProductService,LoanProductService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ISubscriptionRequestService,SubscriptionRequestService>();
builder.Services.AddHttpClient<IApimSubscriptionService,ApimSubscriptionService>();

builder.Services.AddScoped<ILoanProductRepository,LoanProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddScoped<ISubscriptionRequestRepository,SubscriptionRequestRepository>();
// builder.ConfigureFunctionsWebApplication();

// if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
// {
//     builder.Services.AddOpenTelemetry()
//         .UseFunctionsWorkerDefaults()
//         .UseAzureMonitorExporter();
// }

builder.Build().Run();

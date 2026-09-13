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
using Microsoft.Identity.Web;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var scope =
        $"api://{builder.Configuration["AzureAd:ClientId"]}/access_as_user";

    options.AddSecurityDefinition(
        "oauth2",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(
                        $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),

                    TokenUrl = new Uri(
                        $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),

                    Scopes = new Dictionary<string, string>
                    {
                        {
                            scope,
                            "Access Digital Banking API"
                        }
                    }
                }
            }
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    }
                },
                new[] { scope }
            }
        });
});
builder.Services.AddProblemDetails();
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
        builder.Configuration.GetSection("AzureAd"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["Redis:ConnectionString"];
});

builder.Services.AddDbContext<DigitalBankingDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    var connection = new SqlConnection(connectionString);

    var credential = new AzureCliCredential();
    // var credential = new DefaultAzureCredential(
    //     new DefaultAzureCredentialOptions
    //     {
    //         ManagedIdentityClientId = builder.Configuration["ManagedIdentityClientId"]
    //     });

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
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddScoped<ILoanProductRepository,LoanProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Digital Banking API v1");

    options.OAuthClientId(
        "0bc8269b-3608-4384-94f9-10be3d4aef05");

    options.OAuthUsePkce();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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

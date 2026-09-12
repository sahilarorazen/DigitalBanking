using DigitalBanking.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.DAL.Data;

public class DigitalBankingDbContext(DbContextOptions<DigitalBankingDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
    public DbSet<LoanProduct> LoanProducts => Set<LoanProduct>();
    public DbSet<LoanScheme> LoanSchemes => Set<LoanScheme>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoanScheme>()
            .HasOne(ls => ls.LoanProduct)
            .WithMany(lp => lp.LoanSchemes)
            .HasForeignKey(ls => ls.LoanProductId);

        modelBuilder.Entity<LoanScheme>().HasData(

        // Personal Loan
        new LoanScheme
        {
            Id = 1,
            LoanProductId = 1,
            SchemeCode = "PL-SAL",
            SchemeName = "Salaried Personal Loan",
            InterestRate = 10.50M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 2,
            LoanProductId = 1,
            SchemeCode = "PL-SELF",
            SchemeName = "Self Employed Personal Loan",
            InterestRate = 11.00M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 3,
            LoanProductId = 1,
            SchemeCode = "PL-MED",
            SchemeName = "Medical Personal Loan",
            InterestRate = 10.25M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 4,
            LoanProductId = 1,
            SchemeCode = "PL-TRV",
            SchemeName = "Travel Personal Loan",
            InterestRate = 11.50M,
            IsActive = true
        },

        // Home Loan
        new LoanScheme
        {
            Id = 5,
            LoanProductId = 2,
            SchemeCode = "HL-PUR",
            SchemeName = "Home Purchase Loan",
            InterestRate = 8.50M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 6,
            LoanProductId = 2,
            SchemeCode = "HL-CON",
            SchemeName = "Home Construction Loan",
            InterestRate = 8.25M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 7,
            LoanProductId = 2,
            SchemeCode = "HL-REN",
            SchemeName = "Home Renovation Loan",
            InterestRate = 8.75M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 8,
            LoanProductId = 2,
            SchemeCode = "HL-BT",
            SchemeName = "Home Balance Transfer",
            InterestRate = 8.00M,
            IsActive = true
        },

        // Vehicle Loan
        new LoanScheme
        {
            Id = 9,
            LoanProductId = 3,
            SchemeCode = "VL-NCAR",
            SchemeName = "New Car Loan",
            InterestRate = 9.00M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 10,
            LoanProductId = 3,
            SchemeCode = "VL-UCAR",
            SchemeName = "Used Car Loan",
            InterestRate = 10.50M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 11,
            LoanProductId = 3,
            SchemeCode = "VL-BIKE",
            SchemeName = "Two Wheeler Loan",
            InterestRate = 10.25M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 12,
            LoanProductId = 3,
            SchemeCode = "VL-EV",
            SchemeName = "Electric Vehicle Loan",
            InterestRate = 8.90M,
            IsActive = true
        },

        // Business Loan
        new LoanScheme
        {
            Id = 13,
            LoanProductId = 4,
            SchemeCode = "BL-WC",
            SchemeName = "Working Capital Loan",
            InterestRate = 12.00M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 14,
            LoanProductId = 4,
            SchemeCode = "BL-MSME",
            SchemeName = "MSME Business Loan",
            InterestRate = 11.50M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 15,
            LoanProductId = 4,
            SchemeCode = "BL-EQP",
            SchemeName = "Equipment Finance Loan",
            InterestRate = 12.25M,
            IsActive = true
        },
        new LoanScheme
        {
            Id = 16,
            LoanProductId = 4,
            SchemeCode = "BL-STR",
            SchemeName = "Startup Business Loan",
            InterestRate = 13.00M,
            IsActive = true
        });

        modelBuilder.Entity<LoanProduct>().HasData(
        new LoanProduct
        {
            Id = 1,
            ProductCode = "PL",
            ProductName = "Personal Loan",
            IsActive = true
        },
        new LoanProduct
        {
            Id = 2,
            ProductCode = "HL",
            ProductName = "Home Loan",
            IsActive = true
        },
        new LoanProduct
        {
            Id = 3,
            ProductCode = "VL",
            ProductName = "Vehicle Loan",
            IsActive = true
        },
        new LoanProduct
        {
            Id = 4,
            ProductCode = "BL",
            ProductName = "Business Loan",
            IsActive = true
        });
    }
}


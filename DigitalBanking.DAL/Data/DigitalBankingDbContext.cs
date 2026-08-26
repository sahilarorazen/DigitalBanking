using DigitalBanking.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.DAL.Data;

public class DigitalBankingDbContext(DbContextOptions<DigitalBankingDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Account> Accounts => Set<Account>();
}
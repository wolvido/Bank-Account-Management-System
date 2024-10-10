using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BmsKhameleon.Infrastructure.DbContexts
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<MonthlyWorkingBalance> MonthlyWorkingBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .ToTable("Accounts");

            modelBuilder.Entity<Transaction>()
                .ToTable("Transactions");

            modelBuilder.Entity<MonthlyWorkingBalance>()
                .ToTable("MonthlyWorkingBalances");

            modelBuilder.Entity<MonthlyWorkingBalance>()
                .HasIndex(m => new { m.AccountId, m.Date })
                .IsUnique();

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {   
                property.SetColumnType("decimal(18,2)"); 
            }

        }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Entities;

namespace PFMS.DAL.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<TotalTransactionAmount> TotalTransactionAmounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}

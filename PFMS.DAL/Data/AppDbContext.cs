using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.Json;
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
        public DbSet<TotalMonthlyAmount> TotalMonthlyAmounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<TransactionCategory> transactionCategories = new List<TransactionCategory>()
            {
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"),
                    CategoryName = "Travel",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("a2d7aa11-24f2-44c2-85a1-787c95afc34d"),
                    CategoryName = "Food",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"),
                    CategoryName = "Shopping",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("55460ca7-9ea9-4576-b067-18c72d925456"),
                    CategoryName = "Rent and Bills",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("c7b57c96-7034-44de-89aa-2b45323d82cd"),
                    CategoryName = "Home Necessities",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("92ca68b2-e05b-40cf-981b-5abfea29a8c2"),
                    CategoryName = "Others",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString(),
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("bf295b65-d41b-4684-96d5-0a674aa96eb6"),
                    CategoryName = "Salary",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("da49303b-843a-4b6e-acf8-4caf75043afb"),
                    CategoryName = "Sale",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                },
                new TransactionCategory()
                {
                    CategoryId = Guid.Parse("0f1a7152-3c07-4ddc-a370-06ad886995d4"),
                    CategoryName = "Others",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                }
            };

            modelBuilder.Entity<TransactionCategory>().HasData(transactionCategories);
        }
    }
}

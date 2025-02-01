using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Entities;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<TotalTransactionAmount> TotalTransactionAmounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TotalMonthlyAmount> TotalMonthlyAmounts { get; set; }
        public DbSet<CategoryToUser> CategoryToUser { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<OneTimePassword> OneTimePasswords { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<TransactionNotification> TransactionNotifications { get; set; }
        public DbSet<TransactionScreenshot> TransactionScreenshots { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            List<TransactionCategory> transactionCategories = new List<TransactionCategory>()
            {
                new TransactionCategory()
                {
                    Id = Guid.Parse("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"),
                    CategoryName = "Travel",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("a2d7aa11-24f2-44c2-85a1-787c95afc34d"),
                    CategoryName = "Food",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"),
                    CategoryName = "Shopping",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("55460ca7-9ea9-4576-b067-18c72d925456"),
                    CategoryName = "Rent and Bills",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("c7b57c96-7034-44de-89aa-2b45323d82cd"),
                    CategoryName = "Home Necessities",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("92ca68b2-e05b-40cf-981b-5abfea29a8c2"),
                    CategoryName = "Others",
                    TransactionType = Utils.Enums.TransactionType.Expense.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("bf295b65-d41b-4684-96d5-0a674aa96eb6"),
                    CategoryName = "Salary",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("da49303b-843a-4b6e-acf8-4caf75043afb"),
                    CategoryName = "Sale",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                },
                new TransactionCategory()
                {
                    Id = Guid.Parse("0f1a7152-3c07-4ddc-a370-06ad886995d4"),
                    CategoryName = "Others",
                    TransactionType = Utils.Enums.TransactionType.Income.ToString()
                }
            };

            List<CategoryToUser> categoryUsers = new List<CategoryToUser>()
            {
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("0a98cbbb-74a4-42b4-8a4f-f02775b2ce85"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("a2d7aa11-24f2-44c2-85a1-787c95afc34d"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("0c3b10f5-ff22-4f8d-8c14-32908c11efc9"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("55460ca7-9ea9-4576-b067-18c72d925456"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("c7b57c96-7034-44de-89aa-2b45323d82cd"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("92ca68b2-e05b-40cf-981b-5abfea29a8c2"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("bf295b65-d41b-4684-96d5-0a674aa96eb6"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("da49303b-843a-4b6e-acf8-4caf75043afb"),
                    UserId = null
                },
                new CategoryToUser()
                {
                    CategoryId = Guid.Parse("0f1a7152-3c07-4ddc-a370-06ad886995d4"),
                    UserId = null
                }
            };

            List<Role> roles = new List<Role>()
            {
                new Role()
                {
                    Id = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf"),
                    RoleName = "Admin",
                },
                new Role()
                {
                    Id = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                    RoleName = "User"
                }
            };
            

            List<Permission> permissions = new List<Permission>()
            {
                new Permission()
                {
                    Id = Guid.Parse("513eb750-5631-4d99-a440-a97e639c23ff"),
                    PermissionName = PermissionNames.ADD_USER.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("0f14fb1b-8f5d-4225-87cf-cb20dc138bb4"),
                    PermissionName = PermissionNames.UPDATE_PROFILE.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("d1db8d6f-b73a-46cd-8249-60ec84af8fa2"),
                    PermissionName = PermissionNames.GET_USER_PROFILE.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("fc6fec6c-447f-48e9-9d84-bd9a4733c955"),
                    PermissionName = PermissionNames.SEND_OTP.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("b3091aa7-a3c0-497e-8c95-8e12f3271563"),
                    PermissionName = PermissionNames.VERIFY_OTP.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("dafc519b-bda2-45aa-9a3d-9feae7d7f6be"),
                    PermissionName = PermissionNames.RESET_FORGOTTEN_PASSWORD.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("7e7ee692-eed0-45d5-8a78-4dae6cef4161"),
                    PermissionName = PermissionNames.UPDATE_PASSWORD.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("ae54d566-0d6b-4e8e-99cc-5669fb3b5fef"),
                    PermissionName = PermissionNames.GET_ALL_USERS.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("3393955c-25f4-4649-99c5-e4a660eca6ad"),
                    PermissionName = PermissionNames.GET_USER_BY_ID.ToString()
                },
                new Permission()
                {
                    Id = Guid.Parse("8a94c147-2ebb-477c-8166-d1646fd3037b"),
                    PermissionName = PermissionNames.DELETE_USER.ToString()
                }
            };

            var rolesPermissions = new List<Object>()
            {
                new
                {
                    PermissionsId = Guid.Parse("513eb750-5631-4d99-a440-a97e639c23ff"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("0f14fb1b-8f5d-4225-87cf-cb20dc138bb4"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("d1db8d6f-b73a-46cd-8249-60ec84af8fa2"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new                
                {
                    PermissionsId = Guid.Parse("fc6fec6c-447f-48e9-9d84-bd9a4733c955"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("b3091aa7-a3c0-497e-8c95-8e12f3271563"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("dafc519b-bda2-45aa-9a3d-9feae7d7f6be"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("7e7ee692-eed0-45d5-8a78-4dae6cef4161"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("ae54d566-0d6b-4e8e-99cc-5669fb3b5fef"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("3393955c-25f4-4649-99c5-e4a660eca6ad"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },
                new
                {
                    PermissionsId = Guid.Parse("8a94c147-2ebb-477c-8166-d1646fd3037b"),
                    RolesId = Guid.Parse("fb618e31-5e4d-4dea-a1c3-dbd12b86d5cf")
                },

                
                new 
                {
                    PermissionsId = Guid.Parse("0f14fb1b-8f5d-4225-87cf-cb20dc138bb4"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                },
                new 
                {
                    PermissionsId = Guid.Parse("d1db8d6f-b73a-46cd-8249-60ec84af8fa2"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                },
                new 
                {
                    PermissionsId = Guid.Parse("fc6fec6c-447f-48e9-9d84-bd9a4733c955"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                },
                new 
                {
                    PermissionsId = Guid.Parse("b3091aa7-a3c0-497e-8c95-8e12f3271563"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                },
                new 
                {
                    PermissionsId = Guid.Parse("dafc519b-bda2-45aa-9a3d-9feae7d7f6be"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                },
                new 
                {
                    PermissionsId = Guid.Parse("7e7ee692-eed0-45d5-8a78-4dae6cef4161"),
                    RolesId = Guid.Parse("0d8766d8-09df-4aa6-9838-8da59552a736"),
                }
            };

            modelBuilder.Entity<Permission>().HasIndex(x => new { x.PermissionName }).IsUnique();
            modelBuilder.Entity<Permission>().HasData(permissions);
            modelBuilder.Entity<Role>().HasData(roles);

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
                .UsingEntity(x=>x.HasData(rolesPermissions));

            modelBuilder.Entity<TransactionCategory>().HasData(transactionCategories);
            modelBuilder.Entity<CategoryToUser>().HasData(categoryUsers);

            // modelBuilder.Entity<UserRole>().HasKey(x => new { x.Id, x.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}

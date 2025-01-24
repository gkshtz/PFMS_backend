using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        public IBudgetsRepository<BudgetDto> BudgetsRepository { get; }
        public ICategoryRepository<TransactionCategoryDto> CategoriesRepository { get; }
        public IOneTimePasswordsRespository<OneTimePasswordDto> OneTimePasswordsRepository { get; }
        public IRolesRepository<RoleDto> RolesRepository { get; }
        public ITotalTransactionAmountRespository<TotalTransactionAmountDto> TotalTransactionAmountsRespository { get; }
        public ITransactionRepository<TransactionDto> TransactionsRepository { get; }
        public IUserRepository<UserDto> UsersRepository { get; }
        public IScreenshotsRepository<TransactionScreenshotDto> ScreenshotsRepository { get; }
        public Task SaveDatabaseChangesAsync();
    }
}

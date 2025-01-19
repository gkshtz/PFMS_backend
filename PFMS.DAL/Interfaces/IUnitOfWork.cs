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
        public ICategoryRepository CategoriesRepository { get; }
        public IOneTimePasswordsRespository OneTimePasswordsRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public ITotalTransactionAmountRespository TotalTransactionAmountsRespository { get; }
        public ITransactionRepository TransactionsRepository { get; }
        public IUserRepository UsersRepository { get; }
        public IScreenshotsRepository ScreenshotsRepository { get; }
        public Task SaveDatabaseChangesAsync();
    }
}

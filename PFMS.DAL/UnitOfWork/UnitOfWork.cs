using AutoMapper;
using PFMS.DAL.Data;
using PFMS.DAL.Interfaces;
using PFMS.DAL.Repositories;

namespace PFMS.DAL.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private IBudgetsRepository? _budgetsRespository;
        private ICategoryRepository? _categoriesRepository;
        private IRolesRepository? _rolesRepository;
        private IOneTimePasswordsRespository? _otpRespository;
        private ITotalTransactionAmountRespository? _totalTransactionAmountsRepository;
        private ITransactionRepository? _transactionsRepository;
        private IUserRepository? _usersRepository;
        private IScreenshotsRepository? _screenshotsRepository;

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public UnitOfWork(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IBudgetsRepository BudgetsRepository => _budgetsRespository ??= new BudgetsRepository(_appDbContext, _mapper);
        public ICategoryRepository CategoriesRepository => _categoriesRepository ??= new CategoryRepository(_appDbContext, _mapper);
        public IOneTimePasswordsRespository OneTimePasswordsRepository => _otpRespository ??= new OneTimePasswordsRepository(_appDbContext, _mapper);
        public IRolesRepository RolesRepository => _rolesRepository ??= new RolesRepository(_appDbContext, _mapper);
        public ITotalTransactionAmountRespository TotalTransactionAmountsRespository => _totalTransactionAmountsRepository ??= new TotalTransactionAmountRepository(_appDbContext, _mapper);
        public ITransactionRepository TransactionsRepository => _transactionsRepository ??= new TransactionRepository(_appDbContext, _mapper);
        public IUserRepository UsersRepository => _usersRepository ??= new UserRepository(_appDbContext, _mapper);
        public IScreenshotsRepository ScreenshotsRepository => _screenshotsRepository ??= new ScreenshotsRepository(_appDbContext, _mapper);

        public async Task SaveDatabaseChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}

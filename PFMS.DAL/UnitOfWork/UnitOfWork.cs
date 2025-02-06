using AutoMapper;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;
using PFMS.DAL.Repositories;

namespace PFMS.DAL.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private IBudgetsRepository<BudgetDto>? _budgetsRespository;
        private ICategoryRepository<TransactionCategoryDto>? _categoriesRepository;
        private IRolesRepository<RoleDto>? _rolesRepository;
        private IOneTimePasswordsRespository<OneTimePasswordDto>? _otpRespository;
        private ITotalTransactionAmountRespository<TotalTransactionAmountDto>? _totalTransactionAmountsRepository;
        private ITransactionRepository<TransactionDto>? _transactionsRepository;
        private IUserRepository<UserDto>? _usersRepository;
        private IScreenshotsRepository<TransactionScreenshotDto>? _screenshotsRepository;
        private IPermissionsRepository<PermissionDto>? _permissionsRepository;

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public UnitOfWork(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IBudgetsRepository<BudgetDto> BudgetsRepository => _budgetsRespository ??= new BudgetsRepository<BudgetDto, Budget>(_appDbContext, _mapper);
        public ICategoryRepository<TransactionCategoryDto> CategoriesRepository => _categoriesRepository ??= new CategoryRepository<TransactionCategoryDto, TransactionCategory>(_appDbContext, _mapper);
        public IOneTimePasswordsRespository<OneTimePasswordDto> OneTimePasswordsRepository => _otpRespository ??= new OneTimePasswordsRepository<OneTimePasswordDto, OneTimePassword>(_appDbContext, _mapper);
        public IRolesRepository<RoleDto> RolesRepository => _rolesRepository ??= new RolesRepository<RoleDto, Role>(_appDbContext, _mapper);
        public ITotalTransactionAmountRespository<TotalTransactionAmountDto> TotalTransactionAmountsRespository => _totalTransactionAmountsRepository ??= new TotalTransactionAmountRepository<TotalTransactionAmountDto, TotalTransactionAmount>(_appDbContext, _mapper);
        public ITransactionRepository<TransactionDto> TransactionsRepository => _transactionsRepository ??= new TransactionRepository<TransactionDto, Transaction>(_appDbContext, _mapper);
        public IUserRepository<UserDto> UsersRepository => _usersRepository ??= new UserRepository<UserDto, User>(_appDbContext, _mapper);
        public IScreenshotsRepository<TransactionScreenshotDto> ScreenshotsRepository => _screenshotsRepository ??= new ScreenshotsRepository<TransactionScreenshotDto, TransactionScreenshot>(_appDbContext, _mapper);
        public IPermissionsRepository<PermissionDto> PermissionsRepository => _permissionsRepository ??= new PermissionsRepository<PermissionDto, Permission>(_appDbContext, _mapper);

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

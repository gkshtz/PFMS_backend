using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class BudgetsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IBudgetsRepository<Dto>
        where Dto: BudgetDto
        where Entity: Budget
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public BudgetsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year)
        {
            var budget = await _appDbContext.Budgets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Month == month && x.Year == year);
            return _mapper.Map<BudgetDto?>(budget);
        }

        public async Task DeleteBudgetsOfParticularUser(Guid userId)
        {
            List<Budget> budgets = await _appDbContext.Budgets.Where(x => x.UserId == userId).ToListAsync();
            _appDbContext.Budgets.RemoveRange(budgets);
        }
    }
}

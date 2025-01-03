using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class BudgetsRepository: IBudgetsRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbcontext;
        public BudgetsRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbcontext = appDbContext;
            _mapper = mapper;
        }
        public async Task AddBudget(BudgetDto budgetDto)
        {
            var budget = _mapper.Map<Budget>(budgetDto);
            await _appDbcontext.Budgets.AddAsync(budget);
            await _appDbcontext.SaveChangesAsync();
        }

        public async Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year)
        {
            var budget = await _appDbcontext.Budgets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.Month == month && x.Year == year);
            return _mapper.Map<BudgetDto?>(budget);
        }

        public async Task UpdateBudget(BudgetDto budgetDto)
        {
            var budget = _mapper.Map<Budget>(budgetDto);
            _appDbcontext.Budgets.Update(budget);
            await _appDbcontext.SaveChangesAsync();
        }

        public async Task<BudgetDto> GetBudgetById(Guid budgetId)
        {
            var budget = await _appDbcontext.Budgets.AsNoTracking().FirstOrDefaultAsync(x => x.BudgetId == budgetId);
            var budgetDto = _mapper.Map<BudgetDto>(budget);
            return budgetDto;
        }

        public async Task<bool> DeleteBudget(Guid budgetId)
        {
            var budgetDto = await GetBudgetById(budgetId);
            if(budgetDto == null)
            {
                return false;
            }

            var budget = _mapper.Map<Budget>(budgetDto);

            _appDbcontext.Budgets.Remove(budget);
            await _appDbcontext.SaveChangesAsync();

            return true;
        }
    }
}

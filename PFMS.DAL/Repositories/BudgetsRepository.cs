﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var budget = await _appDbcontext.Budgets.FirstOrDefaultAsync(x => x.UserId == userId && x.Month == month && x.Year == year);
            return _mapper.Map<BudgetDto?>(budget);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IBudgetsRepository
    {
        public Task AddBudget(BudgetDto budgetDto);
        public Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year);
        public Task UpdateBudget(BudgetDto budgetDto);
        public Task<BudgetDto> GetBudgetById(Guid budgetId);
        public Task<bool> DeleteBudget(Guid budgetId);
    }
}

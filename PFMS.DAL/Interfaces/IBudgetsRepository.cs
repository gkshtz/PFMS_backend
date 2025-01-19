using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Interfaces
{
    public interface IBudgetsRepository<Dto>: IGenericRepository<Dto>
        where Dto: class, IIdentifiable
    {
        public Task AddBudget(BudgetDto budgetDto);
        public Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year);
        public Task UpdateBudget(BudgetDto budgetDto);
        public Task<BudgetDto> GetBudgetById(Guid budgetId);
        public Task<bool> DeleteBudget(Guid budgetId);
    }
}

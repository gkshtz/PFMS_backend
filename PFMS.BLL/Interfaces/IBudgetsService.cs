using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IBudgetsService
    {
        public Task AddNewBudget(BudgetBo budgetBo, Guid userId);
        public Task<BudgetBo> GetBudget(Guid userId, int month, int year);
        public Task UpdateBudget(BudgetBo budgetBo, Guid userId);
    }
}

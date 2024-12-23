using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IBudgetsService
    {
        public Task AddNewBudget(BudgetBo budgetBo, Guid userId);
    }
}

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
        where Dto: BudgetDto
    {
        public Task<BudgetDto?> GetBudgetByUserId(Guid userId, int month, int year);
    }
}

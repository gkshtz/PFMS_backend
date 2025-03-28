using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IRecurringTransactionsRepository<Dto>: IGenericRepository<Dto>
        where Dto: RecurringTransactionDto
    {
        public Task<IEnumerable<RecurringTransactionDto>> GetAllRecurringTransactions(Guid userId);
    }
}

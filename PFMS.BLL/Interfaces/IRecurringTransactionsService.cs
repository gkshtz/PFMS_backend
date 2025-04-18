﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IRecurringTransactionsService
    {
        public Task AddRecurringTransaction(RecurringTransactionBo recurringTransactionBo, Guid userId);
        public Task<List<RecurringTransactionBo>> GetAllRecurringTransactions(Guid userId);
    }
}

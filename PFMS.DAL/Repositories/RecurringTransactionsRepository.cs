using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class RecurringTransactionsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IRecurringTransactionsRepository<Dto>
        where Dto: RecurringTransactionDto
        where Entity: RecurringTransaction
    {
        public RecurringTransactionsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {

        }
    }
}

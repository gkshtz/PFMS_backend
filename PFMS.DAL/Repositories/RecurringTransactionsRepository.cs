using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.DAL.Data;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class RecurringTransactionsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IRecurringTransactionsRepository<Dto>
        where Dto: RecurringTransactionDto
        where Entity: RecurringTransaction
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        
        public RecurringTransactionsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
    }
}

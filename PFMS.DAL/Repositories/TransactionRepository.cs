using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public TransactionRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.FirstOrDefaultAsync(x => x.UserId == userId);
            /* we dont need to check whether TotalTransactionAmount record for this user is available or not
             because user is already authenticated and a registered user will always have totalTransactionAmount
            Record. So the check is unnecessary */

            var totalTransactionAmountId = totalTransactionAmount!.TotalTransactionAmountId;

            List<Transaction> transactions = await _appDbContext.Transactions.Where(x => x.TotalTransactionAmountId == totalTransactionAmountId).ToListAsync();

            return _mapper.Map<List<TransactionDto>>(transactions);
        }
    }
}

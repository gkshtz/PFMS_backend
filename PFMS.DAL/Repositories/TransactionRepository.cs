using System;
using System.Collections;
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
using PFMS.Utils.Request_Data;

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
        public async Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId, Filter? filter ,Sort? sort)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.FirstOrDefaultAsync(x => x.UserId == userId);
            /* we dont need to check whether TotalTransactionAmount record for this user is available or not
             because user is already authenticated and a registered user will always have totalTransactionAmount
            Record. So the check is unnecessary */

            var totalTransactionAmountId = totalTransactionAmount!.TotalTransactionAmountId;

            IQueryable<Transaction> transactions = _appDbContext.Transactions.Where(x => x.TotalTransactionAmountId == totalTransactionAmountId).AsQueryable();

            #region Apply Filter
            if (filter != null)
            {
                if(filter.FilterOn.Count > filter.FilterQuery.Count)
                {
                    filter.FilterOn = filter.FilterOn.Slice(0, filter.FilterQuery.Count);
                }
                for(int i=0;i<filter.FilterOn.Count;i++)
                {
                    if (filter.FilterQuery[i].Equals("TransactionName", StringComparison.OrdinalIgnoreCase))
                    {
                        transactions = transactions.Where(x => x.TransactionName.Contains(filter.FilterQuery[i].ToString(), StringComparison.OrdinalIgnoreCase));
                    }
                    if (filter.FilterOn[i].Equals("TransactionDescription", StringComparison.OrdinalIgnoreCase))
                    {
                        transactions = transactions.Where(x => x.TransactionDescription != null && x.TransactionDescription.Contains(filter.FilterQuery[i].ToString(), StringComparison.OrdinalIgnoreCase));
                    }
                    if (filter.FilterOn[i].Equals("TaskType", StringComparison.OrdinalIgnoreCase))
                    {
                        transactions = transactions.Where(x => x.TransactionType.ToString() == filter.FilterQuery[i].ToString());
                    }
                }
            }
            #endregion

            #region Apply Sorting
            if (sort!=null)
            {
                if(sort.SortBy.Equals("TransactionName", StringComparison.OrdinalIgnoreCase))
                {
                    if (sort.IsAscending)
                        transactions = transactions.OrderBy(x => x.TransactionName);
                    else
                        transactions = transactions.OrderByDescending(x => x.TransactionName);
                }
                if(sort.SortBy.Equals("TransactionAmount", StringComparison.OrdinalIgnoreCase))
                {
                    if (sort.IsAscending)
                        transactions = transactions.OrderBy(x => x.TransactionAmount);
                    else
                        transactions = transactions.OrderByDescending(x => x.TransactionAmount);
                }
                if(sort.SortBy.Equals("TransactionDate", StringComparison.OrdinalIgnoreCase))
                {
                    if (sort.IsAscending)
                        transactions = transactions.OrderBy(x => x.TransactionDate);
                    else
                        transactions = transactions.OrderByDescending(x => x.TransactionDate);
                }
            }
            #endregion
            var transactionsList = await transactions.ToListAsync();

            return _mapper.Map<List<TransactionDto>>(transactionsList);
        }

        public async Task<TransactionDto> AddTransaction(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            await _appDbContext.Transactions.AddAsync(transaction);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<TotalTransactionAmountDto> GetTotalTransactionAmountByUserId(Guid userId)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmount);
        }
    }
}

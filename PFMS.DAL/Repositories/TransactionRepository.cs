using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.Utils.RequestData;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class TransactionRepository<Dto, Entity>: GenericRepository<Dto, Entity>, ITransactionRepository<Dto>
        where Dto: TransactionDto
        where Entity: Transaction
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public TransactionRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId, Filter? filter ,Sort? sort, Pagination pagination)
        {
            var totalTransactionAmountDto = await GetTotalTransactionAmountByUserId(userId);
            /* we dont need to check whether TotalTransactionAmount record for this user is available or not
             because user is already authenticated and a registered user will always have totalTransactionAmount
            Record. So the check is unnecessary */

            var totalTransactionAmountId = totalTransactionAmountDto!.Id;

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
                    if (filter.FilterOn[i].Equals("TransactionName", StringComparison.OrdinalIgnoreCase))
                    {
                        string filterQuery = filter.FilterQuery[i].ToLower();
                        transactions = transactions.Where(x => x.TransactionName.ToLower().Contains(filterQuery));
                    }
                    if (filter.FilterOn[i].Equals("TransactionDescription", StringComparison.OrdinalIgnoreCase))
                    {
                        string filterQuery = filter.FilterQuery[i].ToLower();
                        transactions = transactions.Where(x => x.TransactionDescription != null && x.TransactionDescription.ToLower().Contains(filterQuery));
                    }
                    if (filter.FilterOn[i].Equals("TransactionType", StringComparison.OrdinalIgnoreCase))
                    {
                        string filterQuery = filter.FilterQuery[i].ToLower();
                        transactions = transactions.Where(x => x.TransactionType.ToLower() == filterQuery);
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

            #region Pagination
            transactions = transactions.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
            #endregion
          
            var transactionsList = await transactions.ToListAsync();

            return _mapper.Map<List<TransactionDto>>(transactionsList);
        }

        public async Task<TotalTransactionAmountDto> GetTotalTransactionAmountByUserId(Guid userId)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmount);
        }

        public async Task<TransactionDto> GetTransactionWithLatestDate(Guid totalTransactionAmounId)
        {
            var transaction = await _appDbContext.Transactions.Where(x => x.TotalTransactionAmountId == totalTransactionAmounId).OrderByDescending(x => x.TransactionDate).FirstOrDefaultAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}

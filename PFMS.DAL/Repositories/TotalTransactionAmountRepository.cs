using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class TotalTransactionAmountRepository<Dto, Entity>: GenericRepository<Dto, Entity> , ITotalTransactionAmountRespository<Dto>
        where Dto: TotalTransactionAmountDto
        where Entity: TotalTransactionAmount
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public TotalTransactionAmountRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<bool> UpdateTotalTransactionAmount(TotalTransactionAmountDto totalTransactionAmountDto)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == totalTransactionAmountDto.Id);
            if(totalTransactionAmount == null)
            {
                return false;
            }
            totalTransactionAmount = _mapper.Map<TotalTransactionAmount>(totalTransactionAmountDto);
            _appDbContext.TotalTransactionAmounts.Update(totalTransactionAmount);
            return true;

        }

        public async Task<TotalMonthlyAmountDto> AddTotalMonthlyAmount(TotalMonthlyAmountDto totalMonthlyAmountDto)
        {
            var totalMonthlyAmount = _mapper.Map<TotalMonthlyAmount>(totalMonthlyAmountDto);
            await _appDbContext.TotalMonthlyAmounts.AddAsync(totalMonthlyAmount);
            return _mapper.Map<TotalMonthlyAmountDto>(totalMonthlyAmount);
        }

        public async Task<TotalMonthlyAmountDto?> GetTotalMonthlyAmountOfParticularMonthAndYear(Guid totalTransactionAmountId, int month, int year)
        {
            var totalMonthlyAmount = await _appDbContext.TotalMonthlyAmounts.FirstOrDefaultAsync(x => x.TotalTransactionAmountId == totalTransactionAmountId && x.Month == month && x.Year == year);
            return _mapper.Map<TotalMonthlyAmountDto?>(totalMonthlyAmount);
        }

        public async Task DeleteTotalMonthlyAmountsByTotalTransactionAmountId(Guid totalTransactionAmountId)
        {
            List<TotalMonthlyAmount> totalMonthlyAmounts = await _appDbContext.TotalMonthlyAmounts.Where(x => x.TotalTransactionAmountId == totalTransactionAmountId)
                .ToListAsync();
            _appDbContext.TotalMonthlyAmounts.RemoveRange(totalMonthlyAmounts);
        }

        public async Task<TotalTransactionAmountDto?> GetTotalTransactionAmountByUserId(Guid userId)
        {
            TotalTransactionAmount? totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmount);
        }

    }
}

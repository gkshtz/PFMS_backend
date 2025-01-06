using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class TotalTransactionAmountRepository: ITotalTransactionAmountRespository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public TotalTransactionAmountRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<bool> UpdateTotalTransactionAmount(TotalTransactionAmountDto totalTransactionAmountDto)
        {
            var totalTransactionAmount = await _appDbContext.TotalTransactionAmounts.AsNoTracking().FirstOrDefaultAsync(x => x.TotalTransactionAmountId == totalTransactionAmountDto.TotalTransactionAmountId);
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
    }
}

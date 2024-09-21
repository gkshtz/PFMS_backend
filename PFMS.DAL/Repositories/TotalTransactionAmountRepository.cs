using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<TotalTransactionAmountDto> UpdateTotalTransactionAmount(TotalTransactionAmountDto totalTransactionAmountDto)
        {   
            var totalTransactionAmount = _mapper.Map<TotalTransactionAmount>(totalTransactionAmountDto);
            _appDbContext.TotalTransactionAmounts.Update(totalTransactionAmount);
            await _appDbContext.SaveChangesAsync();
            return totalTransactionAmountDto;
        }

        public async Task<TotalMonthlyAmountDto> AddTotalMonthlyAmount(TotalMonthlyAmountDto totalMonthlyAmountDto)
        {
            var totalMonthlyAmount = _mapper.Map<TotalMonthlyAmount>(totalMonthlyAmountDto);
            await _appDbContext.TotalMonthlyAmounts.AddAsync(totalMonthlyAmount);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<TotalMonthlyAmountDto>(totalMonthlyAmount);
        }
    }
}

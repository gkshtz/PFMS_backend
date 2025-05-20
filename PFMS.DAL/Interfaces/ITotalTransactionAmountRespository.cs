using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;

namespace PFMS.DAL.Interfaces
{
    public interface ITotalTransactionAmountRespository<Dto>: IGenericRepository<Dto>
        where Dto: TotalTransactionAmountDto
    {
        public Task<TotalMonthlyAmountDto> AddTotalMonthlyAmount(TotalMonthlyAmountDto totalMonthlyAmountDto);

        public Task<TotalMonthlyAmountDto?> GetTotalMonthlyAmountOfParticularMonthAndYear(Guid totalTransactionAmountId, int month, int year);

        public Task DeleteTotalMonthlyAmountsByTotalTransactionAmountId(Guid totalTransactionAmountId);
        public Task<TotalTransactionAmountDto?> GetTotalTransactionAmountByUserId(Guid userId);
    }
}

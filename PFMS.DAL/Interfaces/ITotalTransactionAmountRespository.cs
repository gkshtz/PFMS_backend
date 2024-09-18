using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface ITotalTransactionAmountRespository
    {
        public Task<TotalTransactionAmountDto> UpdateTotalTransactionAmount(TotalTransactionAmountDto totalTransactionAmountDto);
    }
}

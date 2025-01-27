using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IScreenshotsRepository<Dto>: IGenericRepository<Dto>
        where Dto: TransactionScreenshotDto
    {
        public Task<TransactionScreenshotDto> GetScreenshotByTransactionId(Guid transactionId);

        public Task<TotalTransactionAmountDto> GetTotalTransactionAmountByScreenshotId(Guid screenshotId);
    }
}

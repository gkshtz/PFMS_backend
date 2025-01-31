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
    class ScreenshotsRepository<Dto, Entity>: GenericRepository<Dto, Entity> ,IScreenshotsRepository<Dto>
        where Dto: TransactionScreenshotDto
        where Entity: TransactionScreenshot
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public ScreenshotsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<TransactionScreenshotDto> GetScreenshotByTransactionId(Guid transactionId)
        {
            var screenshot = await _appDbContext.TransactionScreenshots.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
            return _mapper.Map<TransactionScreenshotDto>(screenshot);
        }

        public async Task<TotalTransactionAmountDto> GetTotalTransactionAmountByScreenshotId(Guid screenshotId)
        {
            var totalTransactionAmount = await _appDbContext.TransactionScreenshots.Include(x => x.Transaction).Include(x => x.Transaction!.TotalTransactionAmount)
                                                .Where(x => x.Id == screenshotId).Select(x => x.Transaction!.TotalTransactionAmount).FirstOrDefaultAsync();

            return _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmount);
        }
    }   
}

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
    class ScreenshotsRepository: IScreenshotsRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public ScreenshotsRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task AddScreenshot(TransactionScreenshotDto screenshotDto)
        {
            var transactionScreenshot = _mapper.Map<TransactionScreenshot>(screenshotDto);
            await _appDbContext.TransactionScreenshots.AddAsync(transactionScreenshot);
        }

        public async Task<TransactionScreenshotDto> GetScreenshotByTransactionId(Guid transactionId)
        {
            var screenshot = await _appDbContext.TransactionScreenshots.FirstOrDefaultAsync(x => x.TransactionId == transactionId);
            return _mapper.Map<TransactionScreenshotDto>(screenshot);
        }

        public async Task DeleteScreenshot(Guid screenshotId)
        {
            var screenshot = await _appDbContext.TransactionScreenshots.FindAsync(screenshotId);
            if(screenshot!=null)
            {
                _appDbContext.TransactionScreenshots.Remove(screenshot);
            }
        }

        public async Task<bool> UpdateScreenshot(TransactionScreenshotDto screenshotDto)
        {
            var screenshot = await _appDbContext.TransactionScreenshots.AsNoTracking().FirstOrDefaultAsync(x => x.ScreenshotId == screenshotDto.ScreenshotId);
            if (screenshot == null)
            {
                return false;
            }
            screenshot = _mapper.Map<TransactionScreenshot>(screenshotDto);
            _appDbContext.TransactionScreenshots.Update(screenshot);
            return true;
        }
    }   
}

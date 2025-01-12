using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    }   
}

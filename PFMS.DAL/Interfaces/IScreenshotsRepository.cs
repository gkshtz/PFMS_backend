﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IScreenshotsRepository
    {
        public Task AddScreenshot(TransactionScreenshotDto sreenshotDto);
        public Task<TransactionScreenshotDto> GetScreenshotByTransactionId(Guid transactionId);
        public Task DeleteScreenshot(Guid screenshotId);
        public Task<bool> UpdateScreenshot(TransactionScreenshotDto screenshotDto);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IOneTimePasswordsRespository
    {
        public Task AddOtp(OneTimePasswordDto otpDto);
    }
}
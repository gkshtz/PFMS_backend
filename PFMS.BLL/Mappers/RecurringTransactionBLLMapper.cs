﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.DAL.DTOs;

namespace PFMS.BLL.Mappers
{
    public class RecurringTransactionBLLMapper: Profile
    {
        public RecurringTransactionBLLMapper()
        {
            CreateMap<RecurringTransactionBo, RecurringTransactionDto>()
                .ReverseMap();
        }
    }
}

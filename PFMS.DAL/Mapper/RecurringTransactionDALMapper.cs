using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Mapper
{
    public class RecurringTransactionDALMapper: Profile
    {
        public RecurringTransactionDALMapper()
        {
            CreateMap<RecurringTransactionDto, RecurringTransaction>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(dest => dest.TransactionInterval, opt => opt.MapFrom(src => src.TransactionInterval.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => Enum.Parse<TransactionType>(src.TransactionType)))
                .ForMember(dest => dest.TransactionInterval, opt => opt.MapFrom(src => Enum.Parse<TransactionInterval>(src.TransactionInterval)));
        }
    }
}

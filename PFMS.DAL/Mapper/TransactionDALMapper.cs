using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Mapper
{
    public class TransactionDALMapper: Profile
    {
        public TransactionDALMapper()
        {
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.TransactionType, opt=>opt.MapFrom(src=> Enum.Parse<TransactionType>(src.TransactionType)));
        }
    }
}

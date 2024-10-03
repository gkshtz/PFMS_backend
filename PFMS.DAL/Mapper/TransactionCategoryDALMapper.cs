using AutoMapper;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Mapper
{
    public class TransactionCategoryDALMapper: Profile
    {
        public TransactionCategoryDALMapper()
        {
            CreateMap<TransactionCategoryDto, TransactionCategory>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => Enum.Parse<TransactionType>(src.TransactionType)));
        }
    }
}

using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.DAL.DTOs;
using PFMS.Utils.Enums;

namespace PFMS.BLL.Mappers
{
    public class TransactionNotificationBLLMapper: Profile
    {
        public TransactionNotificationBLLMapper()
        {
            CreateMap<TransactionNotificationBo, TransactionNotificationDto>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ReverseMap()
                .ForMember(dest=>dest.TransactionType, opt=>opt.MapFrom(src=>Enum.Parse<TransactionType>(src.TransactionType)));
        }
    }
}

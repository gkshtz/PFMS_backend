using AutoMapper;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;

namespace PFMS.DAL.Mapper
{
    public class TotalTransactionAmountDALMapper: Profile
    {
        public TotalTransactionAmountDALMapper()
        {
            CreateMap<TotalTransactionAmount, TotalTransactionAmountDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class TotalTransactionAmountMapper: Profile
    {
        public TotalTransactionAmountMapper()
        {
            CreateMap<TotalTransactionAmountBo, TotalTransactionAmountModel>();
        }
    }
}

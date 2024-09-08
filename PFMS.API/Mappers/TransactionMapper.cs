using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class TransactionMapper: Profile
    {
        public TransactionMapper()
        {
            CreateMap<TransactionResponseModel, TransactionBo>().ReverseMap();
        }
    }
}

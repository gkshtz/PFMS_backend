using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class TransactionCategoryMapper: Profile
    {
        public TransactionCategoryMapper()
        {
            CreateMap<TransactionCategoryBo, TransactionCategoryResponseModel>();
            CreateMap<TransactionCategoryRequestModel, TransactionCategoryBo>();
        }
    }
}

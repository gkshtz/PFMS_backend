using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class TransactionNotificationMapper: Profile
    {
        public TransactionNotificationMapper()
        {
            CreateMap<TransactionNotificationRequestModel, TransactionNotificationBo>();
            CreateMap<TransactionNotificationBo, TransactionNotificationResponseModel>();
        }
    }
}

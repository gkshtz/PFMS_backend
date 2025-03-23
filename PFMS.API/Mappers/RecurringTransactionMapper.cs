using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class RecurringTransactionMapper: Profile
    {
        public RecurringTransactionMapper()
        {
            CreateMap<RecurringTransactionRequestModel, RecurringTransactionBo>();
            CreateMap<RecurringTransactionBo, RecurringTransactionResponseModel>();
        }
    }
}

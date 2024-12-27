using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class BudgetMapper: Profile
    {
        public BudgetMapper()
        {
            CreateMap<BudgetRequestModel, BudgetBo>();
            CreateMap<BudgetBo, BudgetResponseModel>();
        }
    }
}

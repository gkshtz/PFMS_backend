using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class RoleMapper: Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleBo, RoleResponseModel>();
            CreateMap<RoleRequestModel, RoleBo>();
        }
    }
}

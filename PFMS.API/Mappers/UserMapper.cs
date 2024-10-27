using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            CreateMap<UserRequestModel, UserBo>();
            CreateMap<UserBo, UserResponseModel>();
            CreateMap<UserUpdateRequestModel, UserBo>();           
        }
    }
}

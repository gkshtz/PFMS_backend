using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;

namespace PFMS.API.Mappers
{
    public class UserCredentialsMapper: Profile
    {
        public UserCredentialsMapper()
        {
            CreateMap<UserCredentialsModel, UserCredentialsBo>().ReverseMap();
        }
    }
}

using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserBo> AddUser(UserBo userBo)
        {

        }
    }
}

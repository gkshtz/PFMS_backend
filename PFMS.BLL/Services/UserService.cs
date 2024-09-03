using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserBo> _passwordHasher; 
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<UserBo> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        public async Task<UserBo> AddUserAsync(UserBo userBo)
        {
            TotalTransactionAmountBo totalTransactionAmountBo = new TotalTransactionAmountBo();
            totalTransactionAmountBo.TotalTransactionAmountId = Guid.NewGuid();
            userBo.UserId = Guid.NewGuid();

            // we dont want to store the password in plain text
            userBo.Password = _passwordHasher.HashPassword(null, userBo.Password);

            totalTransactionAmountBo.UserId = userBo.UserId;

            var userDto = _mapper.Map<UserDto>(userBo);
            var totalTransactionAmountDto = _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo);
               
            userDto = await _userRepository.AddUser(userDto, totalTransactionAmountDto);
            return _mapper.Map<UserBo>(userDto);
        }
    }
}

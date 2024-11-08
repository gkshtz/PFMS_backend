using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Constants;
using PFMS.Utils.Custom_Exceptions;

namespace PFMS.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserBo> _passwordHasher;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<UserBo> passwordHasher,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<UserBo> AddUserAsync(UserBo userBo)
        {
            TotalTransactionAmountBo totalTransactionAmountBo = new TotalTransactionAmountBo();
            userBo.UserId = Guid.NewGuid();
            totalTransactionAmountBo.TotalTransactionAmountId = Guid.NewGuid();
            totalTransactionAmountBo.UserId = userBo.UserId;

            // we dont want to store the password in plain text
            userBo.Password = _passwordHasher.HashPassword(null, userBo.Password);

            var userDto = _mapper.Map<UserDto>(userBo);
            var totalTransactionAmountDto = _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo);
               
            userDto = await _userRepository.AddUser(userDto, totalTransactionAmountDto);
            return _mapper.Map<UserBo>(userDto);
        }

        public async Task<string> AuthenticateUser(UserCredentialsBo userCredentialsBo)
        {
            var userDto = await _userRepository.FindUserByEmail(userCredentialsBo.Email);

            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var isAuthenticated = _passwordHasher.VerifyHashedPassword(null, userDto.Password, userCredentialsBo.Password);

            if (isAuthenticated == PasswordVerificationResult.Success)
            {
                var userBo = _mapper.Map<UserBo>(userDto);
                string token = GenerateToken(userBo);
                return token;
            }
            else
            {
                throw new AuthenticationFailedException(ErrorMessages.UserNotAuthenticated);
            }
        }

        public async Task UpdateUserProfile(UserBo userBo, Guid userId)
        {
            var userDto = await _userRepository.GetUserById(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            userBo.UserId = userId;
            userDto = _mapper.Map<UserDto>(userBo);

            await _userRepository.UpdateUser(userDto);
        }

        public async Task UpdatePassword(string oldPassword, string newPassword, Guid userId)
        {
            var userDto = await _userRepository.GetUserById(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var isCorrectOldPassword = _passwordHasher.VerifyHashedPassword(null, userDto.Password, oldPassword);

            if (isCorrectOldPassword == PasswordVerificationResult.Success)
            {
                var newHashedPassword = _passwordHasher.HashPassword(null, newPassword);
                await _userRepository.UpdatePassword(newHashedPassword, userId);
            }
            else
            {
                throw new BadRequestException(ErrorMessages.IncorrectOldPassword);
            }
        }

        public async Task<UserBo> GetUserProfile(Guid userId)
        {
            var userDto = await _userRepository.GetUserProfile(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }
            return _mapper.Map<UserBo>(userDto);
        }

        private string GenerateToken(UserBo userBo)
        {
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]!),
                        new Claim("UserId", userBo.UserId.ToString()!),
                        new Claim("Email",userBo.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signIn
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}

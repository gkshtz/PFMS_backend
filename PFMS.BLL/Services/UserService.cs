using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<UserBo> passwordHasher,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserBo>> GetAllUsers()
        {
            var userDtos = await _userRepository.GetAllUsers();
            return _mapper.Map<List<UserBo>>(userDtos);
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

        public async Task<TokenBo> AuthenticateUser(UserCredentialsBo userCredentialsBo)
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
                TokenBo accessAndRefreshTokens = GenerateToken(userBo);
                return accessAndRefreshTokens;
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

        public async Task<string> RefreshAccessToken()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            var refreshToken = context.Request.Cookies["refresh-token"];
            if(refreshToken == null)
            {
                throw new BadRequestException(ErrorMessages.RefreshTokenIsNotPresnt);
            }

            throw new NotImplementedException();
        }

        private ClaimsPrincipal? ValidateRefreshToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["RefreshToken:Key"]!));

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidAudience = _configuration["RefreshToken:Audience"],
                    ValidIssuer = _configuration["RefreshToken:Issuer"],
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private TokenBo GenerateToken(UserBo userBo)
        {
            //Generate Access Token
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, userBo.UserId.ToString()),
                        new Claim("Email",userBo.Email),
                        new Claim("FirstName", userBo.FirstName),
                        new Claim("LastName", userBo.LastName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signIn
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            //Generate refresh token
            claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userBo.UserId.ToString()),
            };

            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signIn);

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);
            var accessAndRefreshTokens = new TokenBo()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return accessAndRefreshTokens;
        }
    }
}

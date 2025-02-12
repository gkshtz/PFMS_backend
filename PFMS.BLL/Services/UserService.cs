using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.DAL.UnitOfWork;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;

namespace PFMS.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserBo> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IMapper mapper, IPasswordHasher<UserBo> passwordHasher, IConfiguration configuration, IHttpContextAccessor httpContextAccessor
            ,IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserBo>> GetAllUsers()
        {
            var userDtos = await _unitOfWork.UsersRepository.GetAllAsync();
            return _mapper.Map<List<UserBo>>(userDtos);
        }

        public async Task<UserBo> AddUserAsync(UserBo userBo)
        {
            TotalTransactionAmountBo totalTransactionAmountBo = new TotalTransactionAmountBo();
            userBo.Id = Guid.NewGuid();
            totalTransactionAmountBo.Id = Guid.NewGuid();
            totalTransactionAmountBo.UserId = userBo.Id;

            // we dont want to store the password in plain text
            userBo.Password = _passwordHasher.HashPassword(null, userBo.Password);

            var userDto = _mapper.Map<UserDto>(userBo);
            var totalTransactionAmountDto = _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo);

            await _unitOfWork.UsersRepository.AddAsync(userDto);

            await _unitOfWork.TotalTransactionAmountsRespository.AddAsync(totalTransactionAmountDto);

            await _unitOfWork.SaveDatabaseChangesAsync();

            return _mapper.Map<UserBo>(userDto);
        }

        public async Task<TokenBo> AuthenticateUser(UserCredentialsBo userCredentialsBo)
        {
            var userDto = await _unitOfWork.UsersRepository.FindUserByEmail(userCredentialsBo.Email);

            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var isAuthenticated = _passwordHasher.VerifyHashedPassword(null, userDto.Password, userCredentialsBo.Password);

            if (isAuthenticated == PasswordVerificationResult.Success)
            {
                var userBo = _mapper.Map<UserBo>(userDto);
                string accessToken = GenerateAccessToken(userBo);
                string refreshToken = GenerateRefreshToken(userBo);

                var accessAndRefreshTokens = new TokenBo()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return accessAndRefreshTokens;
            }
            else
            {
                throw new AuthenticationFailedException(ErrorMessages.UserNotAuthenticated);
            }
        }

        public async Task UpdateUserProfile(UserBo userBo, Guid userId)
        {
            var userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            userBo.Id = userId;
            userDto = _mapper.Map<UserDto>(userBo);

            await _unitOfWork.UsersRepository.UpdateUser(userDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task UpdatePassword(string oldPassword, string newPassword, Guid userId)
        {
            var userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var isCorrectOldPassword = _passwordHasher.VerifyHashedPassword(null, userDto.Password, oldPassword);

            if (isCorrectOldPassword == PasswordVerificationResult.Success)
            {
                var newHashedPassword = _passwordHasher.HashPassword(null, newPassword);
                await _unitOfWork.UsersRepository.UpdatePassword(newHashedPassword, userId);

                await _unitOfWork.SaveDatabaseChangesAsync();
            }
            else
            {
                throw new BadRequestException(ErrorMessages.IncorrectOldPassword);
            }
        }

        public async Task<UserBo> GetUserProfile(Guid userId)
        {
            var userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }
            return _mapper.Map<UserBo>(userDto);
        }

        public async Task<string> RefreshAccessToken()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            var refreshToken = context.Request.Cookies[ApplicationConstsants.RefreshToken];
            if(refreshToken == null)
            {
                throw new BadRequestException(ErrorMessages.RefreshTokenIsNotPresnt);
            }

            var principal = ValidateRefreshToken(refreshToken);
            if(principal == null)
            {
                context.Response.Cookies.Append(ApplicationConstsants.RefreshToken, refreshToken, new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    HttpOnly = true
                });
                throw new UnauthorizedException(ErrorMessages.InvalidRefreshToken);
            }

            var userId = principal.FindFirst("UserId")?.Value;
            if(userId == null)
            {
                context.Response.Cookies.Append(ApplicationConstsants.RefreshToken, refreshToken, new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    HttpOnly = true
                });
                throw new BadRequestException(ErrorMessages.UserIdNotPresentInRefreshToken);
            }

            var userDto = await _unitOfWork.UsersRepository.GetByIdAsync(Guid.Parse(userId));
            var userBo = _mapper.Map<UserBo>(userDto);

            string accessToken = GenerateAccessToken(userBo);
            return accessToken;
        }

        public void Logout()
        {
            var context = _httpContextAccessor.HttpContext;

            var refreshToken = context.Request.Cookies[ApplicationConstsants.RefreshToken];

            if(refreshToken == null)
            {
                return;
            }

            context.Response.Cookies.Append(ApplicationConstsants.RefreshToken, refreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/users/refresh-token",
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            //check if the user exists with this userId
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            // remove all the budgets of the user
            await _unitOfWork.BudgetsRepository.DeleteBudgetsOfParticularUser(userId);

            // remove all the screenshots of the user
            List<TransactionScreenshotDto> screenshotDtos = await _unitOfWork.ScreenshotsRepository.DeleteAndGetScreenshotsByUserId(userId);
            var screenshotBos = _mapper.Map<List<TransactionScreenshotBo>>(screenshotDtos);

            screenshotBos.ForEach(screenshotBo => File.Delete(screenshotBo.FilePath));

            Guid totalTransactionAmountId = (await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId)).Id;

            // remove all the transactions of the user
            await _unitOfWork.TransactionsRepository.DeleteTransactionsByTotalTransactionAmountId(totalTransactionAmountId);

            // remove all the total monthly amounts of the user
            await _unitOfWork.TotalTransactionAmountsRespository.DeleteTotalMonthlyAmountsByTotalTransactionAmountId(totalTransactionAmountId);

            // remove total transaction amount of the user
            await _unitOfWork.TotalTransactionAmountsRespository.DeleteAsync(totalTransactionAmountId);

            // remove all the user roles
            await _unitOfWork.RolesRepository.DeleteAllRolesAssignedToUser(userId);

            // remove all the OTPs of the user
            await _unitOfWork.OneTimePasswordsRepository.DeleteAllOtpsOfUser(userId);

            // remove all the transaction notifications of the user
            await _unitOfWork.TransactionNotificationsRepository.DeleteAllNotificationsOfUser(userId);

            // remove the transaction categories which were added by the user
            IEnumerable<Guid> categoryIds = await _unitOfWork.CategoriesRepository.DeleteAllCategoryToUserByUserId(userId);
            await _unitOfWork.CategoriesRepository.DeleteAllCategoriesByCategoryIds(categoryIds);

            // remove the user
            await _unitOfWork.UsersRepository.DeleteAsync(userId);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        #region Helper Functions
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
        private string GenerateAccessToken(UserBo userBo)
        {
            //Generate Access Token
            var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                        new Claim("UserId", userBo.Id.ToString()),
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

            return accessToken;
        }
        private string GenerateRefreshToken(UserBo userBo)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId", userBo.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["RefreshToken:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["RefreshToken:Issuer"],
                _configuration["RefreshToken:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: signIn);

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return refreshToken;
        }
        #endregion
    }
}

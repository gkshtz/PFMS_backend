using System.Net;
using System.Security.Cryptography.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.BLL.Services;
using PFMS.Utils.Constants;
using PFMS.Utils.Enums;
using PFMS.API.ActionFilters;

namespace PFMS.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOneTimePasswordsService _otpService;

        public UserController(IUserService userService, IMapper mapper, IOneTimePasswordsService otpService)
        {
            _userService = userService;
            _otpService = otpService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("list")]
        [AllowedRole(RoleNames.ADMIN)]
        public async Task<IActionResult> GetAllAsync()
        {
            var userBos = await _userService.GetAllUsers();
            var userModels = _mapper.Map<List<UserResponseModel>>(userBos);
            var response = new GenericSuccessResponse<List<UserResponseModel>>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = userModels,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [AllowedRole(RoleNames.ADMIN)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userBo = await _userService.GetUserProfile(id);
            var userModel = _mapper.Map<UserResponseModel>(userBo);
            var response = new GenericSuccessResponse<UserResponseModel>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = userModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var userBo = await _userService.GetUserProfile(UserId);
            var userModel = _mapper.Map<UserResponseModel>(userBo);
            var response = new GenericSuccessResponse<UserResponseModel>()
            {
                StatusCode = 200,
                ResponseData = userModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPost]
        [AllowedRole(RoleNames.ADMIN)]
        public async Task<IActionResult> AddAsync([FromBody] UserRequestModel userRequestModel)
        {
            var userBo = _mapper.Map<UserBo>(userRequestModel);
            userBo = await _userService.AddUserAsync(userBo);
            UserResponseModel userResponseModel = _mapper.Map<UserResponseModel>(userBo);
            GenericSuccessResponse<UserResponseModel> response = new GenericSuccessResponse<UserResponseModel>()
            {
                StatusCode = 201,
                ResponseData = userResponseModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return CreatedAtAction(nameof(GetById), new { id = userResponseModel.UserId }, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredentialsModel credentialsModel)
        {
            var credentialsBo = _mapper.Map<UserCredentialsBo>(credentialsModel);
            TokenBo accessAndRefreshToken = await _userService.AuthenticateUser(credentialsBo);
            var accessToken = accessAndRefreshToken.AccessToken;
            var refreshToken = accessAndRefreshToken.RefreshToken;
            GenericSuccessResponse<string> response = new GenericSuccessResponse<string>()
            {
                StatusCode = 200,
                ResponseData = accessToken,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            Response.Cookies.Append(ApplicationConstsants.RefreshToken, refreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/users/refresh-token",
                Expires = DateTime.UtcNow.AddHours(12)
            });


            return Ok(response);
        }

        [HttpPatch]
        [Route("profile")]
        public async Task<IActionResult> PatchAsync([FromBody] UserUpdateRequestModel userModel)
        {
            var userBo = _mapper.Map<UserBo>(userModel);
            await _userService.UpdateUserProfile(userBo, UserId);
            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateRequestModel passwordUpdateModel)
        {
            await _userService.UpdatePassword(passwordUpdateModel.OldPassword, passwordUpdateModel.NewPassword, UserId);

            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("refresh-token/refresh")]
        public async Task<IActionResult> GetRefreshedAccessToken()
        {
            var newAccessToken = await _userService.RefreshAccessToken();
            var response = new GenericSuccessResponse<string>()
            {
                StatusCode = 200,
                ResponseData = newAccessToken,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("refresh-token/logout")]
        public async Task<IActionResult> Logout()
        {
            _userService.Logout();
            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("otp/send")]
        public async Task<IActionResult> SendOtpAsync([FromBody] SendOtpRequest otpRequest)
        {
            var uniqueDeviceId = await _userService.GenerateAndSendOtp(otpRequest.EmailAddress);
            Response.Cookies.Append(ApplicationConstsants.UniqueDeviceId, uniqueDeviceId.ToString(), new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/users/otp",
                Expires = DateTime.UtcNow.AddMinutes(7)
            });

            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("otp/verify")]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] VerifyOtpRequestModel verifyOtpModel)
        {
            await _otpService.VerifyOtp(verifyOtpModel.Otp, verifyOtpModel.EmailAddress);
            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("otp/reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestModel resetPasswordModel)
        {
            await _otpService.ResetPassword(resetPasswordModel.NewPassword);
            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

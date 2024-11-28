using System.Net;
using System.Security.Cryptography.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.BLL.Services;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("list")]
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

            Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Path = "/refresh",
                Expires = DateTime.UtcNow.AddHours(1)
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
        public async Task<IActionResult> GetRefreshedAccessToken()
        {
            
        }
    }
}

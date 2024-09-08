using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredentialsModel credentialsModel)
        {
            var credentialsBo = _mapper.Map<UserCredentialsBo>(credentialsModel);
            string token = await _userService.AuthenticateUser(credentialsBo);
            GenericSuccessResponse<string> response = new GenericSuccessResponse<string>()
            {
                StatusCode = 200,
                ResponseData = token,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

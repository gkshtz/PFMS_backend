using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;

namespace PFMS.API.Controllers
{
    [Route("api/[controller]")]
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
            return Ok(userResponseModel);
        }
    }
}

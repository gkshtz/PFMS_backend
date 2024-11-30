using System.Diagnostics;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;
        public RolesController(IRolesService rolesService, IMapper mapper)
        {
            _rolesService = rolesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var roleBos = await _rolesService.GetAllRoles();
            var roleModels = _mapper.Map<List<RoleResponseModel>>(roleBos);

            var response = new GenericSuccessResponse<List<RoleResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = roleModels,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RoleRequestModel roleModel)
        {
            var roleBo = _mapper.Map<RoleBo>(roleModel);
            await _rolesService.AddRole(roleBo);
            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("user-roles")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleModel userRoleModel)
        {
            var userRoleBo = _mapper.Map<UserRoleBo>(userRoleModel);
            await _rolesService.AddUserRole(userRoleBo);
            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

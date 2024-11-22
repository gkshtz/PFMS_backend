using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
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
    }
}

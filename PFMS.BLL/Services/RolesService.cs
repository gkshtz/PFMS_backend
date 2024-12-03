using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class RolesService: IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;
        public RolesService(IRolesRepository rolesRepository, IMapper mapper)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }
        public async Task<List<RoleBo>> GetAllRoles()
        {
            var roleDtos = await _rolesRepository.GetAllRoles();
            return _mapper.Map<List<RoleBo>>(roleDtos);
        }

        public async Task AddRole(RoleBo roleBo)
        {
            roleBo.RoleId = Guid.NewGuid();
            var roleDto = _mapper.Map<RoleDto>(roleBo);
            await _rolesRepository.AddRole(roleDto);
        }

        public async Task AddUserRole(UserRoleBo userRoleBo)
        {
            var userRoleDto = _mapper.Map<UserRoleDto>(userRoleBo);
            await _rolesRepository.AddUserRole(userRoleDto);
        }

        public async Task<List<string>> GetRoleNamesAssignedToUser(Guid userId)
        {
            List<string> roleNames = await _rolesRepository.GetRoleNamesAssignedToUser(userId);
            return roleNames;
        }
    }
}

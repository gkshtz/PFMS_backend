using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class RolesService: IRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RolesService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<RoleBo>> GetAllRoles()
        {
            var roleDtos = await _unitOfWork.RolesRepository.GetAllRoles();
            return _mapper.Map<List<RoleBo>>(roleDtos);
        }

        public async Task AddRole(RoleBo roleBo)
        {
            roleBo.RoleId = Guid.NewGuid();
            var roleDto = _mapper.Map<RoleDto>(roleBo);
            await _unitOfWork.RolesRepository.AddRole(roleDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task AddUserRole(UserRoleBo userRoleBo)
        {
            var userRoleDto = _mapper.Map<UserRoleDto>(userRoleBo);
            await _unitOfWork.RolesRepository.AddUserRole(userRoleDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task<List<string>> GetRoleNamesAssignedToUser(Guid userId)
        {
            List<string> roleNames = await _unitOfWork.RolesRepository.GetRoleNamesAssignedToUser(userId);
            return roleNames;
        }
    }
}

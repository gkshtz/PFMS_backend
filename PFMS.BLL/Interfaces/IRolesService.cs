using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IRolesService
    {
        public Task<List<RoleBo>> GetAllRoles();

        public Task AddRole(RoleBo roleBo);

        public Task AddUserRole(UserRoleBo userRoleBo);

        public Task<List<string>> GetRoleNamesAssignedToUser(Guid userId);

        public Task<List<RoleBo>> GetRolesAssignedToUser(Guid userId);
    }
}

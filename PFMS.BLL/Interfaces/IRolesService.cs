using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IRolesService
    {
        public Task<List<RoleBo>> GetAllRoles();

        public Task AddRole(RoleBo roleBo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IRolesRepository<Dto>: IGenericRepository<Dto>
        where Dto: RoleDto
    {
        public Task AddUserRole(UserRoleDto userRoleDto);

        public Task<List<string>> GetRoleNamesAssignedToUser(Guid userId);

        public Task<List<RoleDto>> GetRolesAssignedToUser(Guid userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;
namespace PFMS.DAL.Interfaces
{
    public interface IPermissionsRepository<Dto>: IGenericRepository<Dto>
        where Dto: PermissionDto
    {
        public Task<List<PermissionDto>> GetPermissionsAssignedToRoleIds(IEnumerable<Guid> roleIds);
    }
}

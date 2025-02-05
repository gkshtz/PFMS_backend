using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class PermissionsRepository<Dto, Entity>: GenericRepository<Dto, Entity>,  IPermissionsRepository<Dto>
        where Dto: PermissionDto
        where Entity: Permission
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public PermissionsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        public async Task<List<PermissionDto>> GetPermissionsAssignedToRoleIds(IEnumerable<Guid> roleIds)
        {
            List<Permission> permissions = await _appDbContext.Permissions.Include(x => x.Roles).Where(x => x.Roles.Any(role => roleIds.Contains(role.Id))).ToListAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}

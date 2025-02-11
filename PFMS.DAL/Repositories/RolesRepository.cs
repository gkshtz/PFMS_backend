using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class RolesRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IRolesRepository<Dto>
        where Entity: Role
        where Dto: RoleDto
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RolesRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task AddUserRole(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            await _appDbContext.UserRoles.AddAsync(userRole);
        }

        public async Task<List<string>> GetRoleNamesAssignedToUser(Guid userId)
        {
            var roleNames = await _appDbContext.UserRoles.Include(x => x.Role).Where(x => x.UserId == userId).Select(x => x.Role!.RoleName).ToListAsync();
            return roleNames;
        }

        public async Task<List<RoleDto>> GetRolesAssignedToUser(Guid userId)
        {
            List<Role> roles = await _appDbContext.UserRoles.Include(x => x.Role).Where(x => x.UserId == userId).Select(x => x.Role!).ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task DeleteAllRolesAssignedToUser(Guid userId)
        {
            List<UserRole> userRoles = await _appDbContext.UserRoles.Where(x => x.UserId == userId).ToListAsync();
            _appDbContext.UserRoles.RemoveRange(userRoles);
        }
    }
}

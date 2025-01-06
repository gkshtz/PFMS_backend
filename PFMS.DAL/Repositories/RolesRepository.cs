using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class RolesRepository: IRolesRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public RolesRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllRoles()
        {
            var roles = await _appDbContext.Roles.ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task AddRole(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _appDbContext.Roles.AddAsync(role);
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IRolesRepository
    {
        public Task<List<RoleDto>> GetAllRoles();

        public Task AddRole(RoleDto roleDto);

        public Task AddUserRole(UserRoleDto userRoleDto);

        public Task<List<string>> GetRoleNamesAssignedToUser(Guid userId);
    }
}

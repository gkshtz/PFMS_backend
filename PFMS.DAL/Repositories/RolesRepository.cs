﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
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
    }
}
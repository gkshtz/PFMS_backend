using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class RolesService: IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;
        public RolesService(IRolesRepository rolesRepository, IMapper mapper)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }
        public async Task<List<RoleBo>> GetAllRoles()
        {
            var roleDtos = await _rolesRepository.GetAllRoles();
            return _mapper.Map<List<RoleBo>>(roleDtos);
        }
    }
}

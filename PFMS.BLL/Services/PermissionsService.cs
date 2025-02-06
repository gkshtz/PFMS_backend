using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class PermissionsService: IPermissionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PermissionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<PermissionBo>> GetPermissionsAssignedToRoleIds(IEnumerable<Guid> roleIds)
        {
            List<PermissionDto> permissionDtos = await _unitOfWork.PermissionsRepository.GetPermissionsAssignedToRoleIds(roleIds);
            return _mapper.Map<List<PermissionBo>>(permissionDtos);
        }
    }
}

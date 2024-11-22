using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;

namespace PFMS.DAL.Mapper
{
    public class RoleDALMapper: Profile
    {
        public RoleDALMapper()
        {
            CreateMap<RoleDto, Role>().ReverseMap();
        }
    }
}

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
    public class OneTimePasswordDALMapper: Profile
    {
        public OneTimePasswordDALMapper()
        {
            CreateMap<OneTimePasswordDto, OneTimePassword>();
        }
    }
}

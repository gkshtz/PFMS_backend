using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class OneTimePasswordsRepository: IOneTimePasswordsRespository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public OneTimePasswordsRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task AddOtp(OneTimePasswordDto otpDto)
        {
            var otp = _mapper.Map<OneTimePassword>(otpDto);
            await _appDbContext.OneTimePasswords.AddAsync(otp);
            await _appDbContext.SaveChangesAsync();
        }
    }
}

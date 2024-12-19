using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OneTimePasswordDto> FetchByOtp(string otp)
        {
            var oneTimePassword = await _appDbContext.OneTimePasswords.AsNoTracking().FirstOrDefaultAsync(x => x.Otp == otp);
            var otpDto = _mapper.Map<OneTimePasswordDto>(oneTimePassword);
            return otpDto;
        }

        public async Task UpdateOtp(OneTimePasswordDto otpDto)
        {
            var otp = _mapper.Map<OneTimePassword>(otpDto);
            _appDbContext.OneTimePasswords.Update(otp);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<OneTimePasswordDto> FetchByUniqueDeviceId(Guid uniqueDeviceId)
        {
            var oneTimePassword = await _appDbContext.OneTimePasswords.AsNoTracking().FirstOrDefaultAsync(x => x.UniqueDeviceId == uniqueDeviceId);
            var otpDto = _mapper.Map<OneTimePasswordDto>(oneTimePassword);
            return otpDto;
        }
    }
}

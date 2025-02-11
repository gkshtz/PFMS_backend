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
    public class OneTimePasswordsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IOneTimePasswordsRespository<Dto>
        where Dto: OneTimePasswordDto
        where Entity: OneTimePassword
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public OneTimePasswordsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<OneTimePasswordDto> FetchByOtp(string otp)
        {
            var oneTimePassword = await _appDbContext.OneTimePasswords.AsNoTracking().FirstOrDefaultAsync(x => x.Otp == otp);
            var otpDto = _mapper.Map<OneTimePasswordDto>(oneTimePassword);
            return otpDto;
        }

        public async Task<OneTimePasswordDto> FetchByUniqueDeviceId(Guid uniqueDeviceId)
        {
            var oneTimePassword = await _appDbContext.OneTimePasswords.AsNoTracking().FirstOrDefaultAsync(x => x.UniqueDeviceId == uniqueDeviceId);
            var otpDto = _mapper.Map<OneTimePasswordDto>(oneTimePassword);
            return otpDto;
        }

        public async Task DeleteAllOtpsOfUser(Guid userId)
        {
            List<OneTimePassword> otps = await _appDbContext.OneTimePasswords.Where(x => x.UserId == userId).ToListAsync();
            _appDbContext.OneTimePasswords.RemoveRange(otps);
        }
    }
}

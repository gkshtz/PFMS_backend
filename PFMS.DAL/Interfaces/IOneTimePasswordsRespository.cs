using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IOneTimePasswordsRespository<Dto>: IGenericRepository<Dto>
        where Dto: OneTimePasswordDto
    {
        public Task<OneTimePasswordDto> FetchByOtp(string otp);

        public Task<OneTimePasswordDto> FetchByUniqueDeviceId(Guid uniqueDeviceId);

        public Task DeleteAllOtpsOfUser(Guid userId);
    }
}

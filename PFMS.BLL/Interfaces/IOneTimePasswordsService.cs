using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.BLL.Interfaces
{
    public interface IOneTimePasswordsService
    {
        public Task VerifyOtp(string otp, string email);
    }
}

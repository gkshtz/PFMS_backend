using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;

namespace PFMS.Utils.Custom_Exceptions
{
    public class AuthenticationFailedException: CustomException
    {
        public AuthenticationFailedException(): base(500)
        {
            Name = ErrorNames.AUTHENTICATION_FAILED_ERROR.ToString();
        }
        public AuthenticationFailedException(string message): base(message, 500)
        {
            Name = ErrorNames.AUTHENTICATION_FAILED_ERROR.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;

namespace PFMS.Utils.CustomExceptions
{
    public class UnauthorizedException: CustomException
    {
        public UnauthorizedException(): base(401)
        {
            Name = ErrorNames.UNAUTHORIZED_ERROR.ToString();
        }
        public UnauthorizedException(string message): base(message, 401)
        {
            Name = ErrorNames.UNAUTHORIZED_ERROR.ToString();
        }
    }
}

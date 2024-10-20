using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;

namespace PFMS.Utils.Custom_Exceptions
{
    public class ForbiddenException: CustomException
    {
        public ForbiddenException(string message): base(message, 403)
        {
            Name = ErrorNames.FORBIDDEN_ERROR.ToString();
        }
        public ForbiddenException(): base(403)
        {
            Name = ErrorNames.FORBIDDEN_ERROR.ToString();
        }
    }
}

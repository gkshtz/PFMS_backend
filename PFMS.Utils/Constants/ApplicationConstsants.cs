using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.Utils.Constants
{
    public static class ApplicationConstsants
    {
        public static readonly List<string> BypassRoutes = new List<string>()
        {
            "/api/users/login"
        };
    }
}

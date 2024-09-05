using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.Utils.Custom_Exceptions
{
    public class CustomException: Exception
    {
        public int StatusCode { get; set; }
        public string Name { get; set; }
        public CustomException(int status)
        {
            StatusCode = status;
        }
        public CustomException(string message, int status): base(message)
        {
            StatusCode = status;
        }
    }
}

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.Utils.Request_Data
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public Pagination()
        {
            PageNumber = 1;
            PageSize = 1000;
        }
    }
}

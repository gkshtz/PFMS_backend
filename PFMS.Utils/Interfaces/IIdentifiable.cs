using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.Utils.Interfaces
{
    public interface IIdentifiable
    {
         public Guid Id { get; set; }
    }
}

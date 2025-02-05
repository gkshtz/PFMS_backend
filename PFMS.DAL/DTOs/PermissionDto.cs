using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class PermissionDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public required string PermissionName { get; set; }
    }
}

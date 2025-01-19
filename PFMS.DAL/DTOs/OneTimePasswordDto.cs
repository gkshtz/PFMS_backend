using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class OneTimePasswordDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public string Otp { get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
        public bool IsVerified { get; set; }
        public Guid UniqueDeviceId { get; set; }
    }
}

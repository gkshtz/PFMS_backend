using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class UserDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }
        public string Password { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public string? City { get; set; }

        public string Password { get; set; }
    }
}

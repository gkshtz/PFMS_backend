﻿namespace PFMS.BLL.BOs
{
    public class UserBo
    {
        public Guid UserId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string? City { get; set; }

        public string Password { get; set; }
    }
}

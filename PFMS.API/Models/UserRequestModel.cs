using System.ComponentModel.DataAnnotations.Schema;

namespace PFMS.API.Models
{
    public class UserRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public string? City { get; set; }

        public string Password { get; set; }
    }
}

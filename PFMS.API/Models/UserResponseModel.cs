using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class UserResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string? City { get; set; }
    }
}

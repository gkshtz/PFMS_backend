namespace PFMS.API.Models
{
    public class UserUpdateRequestModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
    }
}

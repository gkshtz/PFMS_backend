using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class UserCredentialsModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = ErrorMessages.ShortPassword)]
        public string Password { get; set; }
    }
}

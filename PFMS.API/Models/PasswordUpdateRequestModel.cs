using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class PasswordUpdateRequestModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}

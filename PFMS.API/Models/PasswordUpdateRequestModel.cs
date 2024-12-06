using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class PasswordUpdateRequestModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = ErrorMessages.ShortPassword)]
        public string NewPassword { get; set; }
    }
}

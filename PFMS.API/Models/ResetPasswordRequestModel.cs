using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [MinLength(8, ErrorMessage = ErrorMessages.ShortPassword)]
        public string NewPassword { get; set; }
    }
}

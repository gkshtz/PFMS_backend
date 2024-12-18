using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class VerifyOtpRequestModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = ErrorMessages.OtpShouldContainDigits)]
        [StringLength(6, MinimumLength = 6, ErrorMessage = ErrorMessages.OtpLengthShouldBeSix)]
        public string Otp { get; set; }
    }
}

using PFMS.Utils.Constants;
using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class UserUpdateRequestModel
    {
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ErrorMessages.FieldMustContainsAlphabeticCharacters)]
        [Required]
        public string FirstName { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = ErrorMessages.FieldMustContainsAlphabeticCharacters)]
        [Required]
        public string LastName { get; set; }

        [Range(16, 100, ErrorMessage = ErrorMessages.OutOfAgeRange)]
        [Required]
        public int Age { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string? City { get; set; }
    }
}

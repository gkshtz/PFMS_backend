using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class UserRequestModel
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

        [MinLength(8, ErrorMessage = ErrorMessages.ShortPassword)]
        public string Password { get; set; }
    }
}

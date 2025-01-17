using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;

namespace PFMS.API.CustomValidators
{
    public class ValidateScreenshot: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null)
            {
                return true;
            }

            var file = (IFormFile)value;

            // check the file size
            if(file.Length > ApplicationConstsants.MaximumScreenshotSize)
            {
                return false;
            }    
            // check the extension of the file
            if(!ApplicationConstsants.AllowedExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                return false;
            }
            return true;
        }
    }
}

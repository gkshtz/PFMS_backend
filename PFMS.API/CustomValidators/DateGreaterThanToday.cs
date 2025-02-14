using System.ComponentModel.DataAnnotations;

namespace PFMS.API.CustomValidators
{
    public class DateGreaterThanToday: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value is DateOnly date)
            {
                if(date <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}

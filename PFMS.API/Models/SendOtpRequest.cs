using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class SendOtpRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}

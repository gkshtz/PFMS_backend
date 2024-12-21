using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class SendOtpRequestModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}

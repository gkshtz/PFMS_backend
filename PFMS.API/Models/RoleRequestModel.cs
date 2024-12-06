using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace PFMS.API.Models
{
    public class RoleRequestModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

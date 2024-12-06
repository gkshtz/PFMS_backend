using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class UserRoleModel
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}

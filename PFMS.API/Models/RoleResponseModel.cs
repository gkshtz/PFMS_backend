using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class RoleResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}

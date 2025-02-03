using Microsoft.AspNetCore.Mvc.Filters;
using PFMS.BLL.Interfaces;

namespace PFMS.API.ActionFilters
{
    public class PermissionRequired: ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //fetch the userId from the claims
            var httpContext = context.HttpContext;
            var userId = httpContext.User.FindFirst("UserId")!.Value;

            //inject the IRolesService object
            IRolesService rolesService = httpContext.RequestServices.GetService<IRolesService>()!;

            // get the roleIds assigned to the user
            IEnumerable<Guid> roleIds = (await rolesService.GetRolesAssignedToUser(Guid.Parse(userId))).Select(x => x.Id);
        }
    }
}

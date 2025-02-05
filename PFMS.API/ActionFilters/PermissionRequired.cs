using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PFMS.BLL.Interfaces;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Constants;
using Microsoft.Identity.Client;
namespace PFMS.API.ActionFilters
{
    public class PermissionRequired : ActionFilterAttribute
    {
        private readonly string _requiredPermission;
        public PermissionRequired(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            HttpContext httpContext = context.HttpContext;

            if (!Guid.TryParse(httpContext.User.FindFirst("UserId")?.Value, out Guid userId))
            {
                throw new BadRequestException(ErrorMessages.UserIdClaimsNotPresent);
            }

            IRolesService rolesService = httpContext.RequestServices.GetService<IRolesService>()!;
            IPermissionsService permissionsService = httpContext.RequestServices.GetService<IPermissionsService>()!;

            IEnumerable<Guid> roleIds = (await rolesService.GetRolesAssignedToUser(userId)).Select(x => x.Id);

            IEnumerable<string> permissionNames = (await permissionsService.GetPermissionsAssignedToRoleIds(roleIds)).Select(x => x.PermissionName);
            

            if(!permissionNames.Contains(_requiredPermission))
            {
                throw new ForbiddenException(ErrorMessages.ActionNotAllowed);
            }

            await base.OnActionExecutionAsync(context, next);
        }
        
    }
}
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;

namespace PFMS.API.ActionFilters
{
    public class AllowedRole: ActionFilterAttribute
    {
        private readonly RoleNames _requiredRole;
        public AllowedRole(RoleNames requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Why OnActionExecutionAsync in place of OnActionExecuting - When you make a method async void, the exception handling behavior
            //is different. An exception thrown in an async void method doesn't propagate back through the pipeline in the same way,
            //and it can be "lost" or not handled by the middleware.

            //The OnActionExecutionAsync method is the proper way to handle asynchronous logic in action filters.
            //This method allows exceptions to propagate correctly through the request pipeline and be caught by your middleware
            //because it uses a Task return type and awaits asynchronous operations properly.

            var httpContext = context.HttpContext;

            var userId = httpContext.User.FindFirst("UserId")!.Value;

            var _rolesService = httpContext.RequestServices.GetService<IRolesService>();

            List<string> roleNames = await _rolesService!.GetRoleNamesAssignedToUser(Guid.Parse(userId));

            if(!roleNames.Any(role => role.Equals(_requiredRole.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new ForbiddenException(ErrorMessages.ActionNotAllowed);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}

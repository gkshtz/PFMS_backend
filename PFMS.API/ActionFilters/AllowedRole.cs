using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;

namespace PFMS.API.ActionFilters
{
    public class AllowedRole: ActionFilterAttribute
    {
        private readonly string _requiredRole;
        public AllowedRole(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}

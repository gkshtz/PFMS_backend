using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.Utils.Request_Data;

namespace PFMS.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public Filter? Filter { get; set; }
        [NonAction]
        public void FetchFilters()
        {
            var query = HttpContext.Request.Query;
            if(query.ContainsKey("filterOn"))
            {
                Filter = new Filter()
                {
                    FilterOn = query["filterOn"].ToString().Split(',').ToList(),
                    FilterQuery = query["filterQuery"].ToString().Split(',').ToList()
                };
            }
        }
    }
}

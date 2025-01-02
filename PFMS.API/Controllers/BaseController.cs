using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PFMS.Utils.RequestData;

namespace PFMS.API.Controllers
{
    public class BaseController : ControllerBase, IActionFilter
    { 
        public void OnActionExecuting(ActionExecutingContext context)
        {
            UserId = Guid.Parse(User.FindFirst("UserId")?.Value ?? Guid.Empty.ToString());
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public Guid UserId { get; set; }
        public Filter? Filter { get; set; }
        public Sort? Sort { get; set; }
        public Pagination? Pagination { get; set; }

        [NonAction]
        public void FetchParameters()
        {
            FetchFilters();
            FetchSort();
            FetchPagination();
        }

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

        [NonAction]
        public void FetchSort()
        {
            var query = HttpContext.Request.Query;
            if(query.ContainsKey("sortBy"))
            {
                Sort = new Sort();
                Sort.SortBy = query["sortBy"].ToString();
                if (query.ContainsKey("isAscending"))
                {
                    Sort.IsAscending = bool.Parse(query["isAscending"]!);
                }
                else
                {
                    Sort.IsAscending = true;
                }
            }
        }

        [NonAction]
        public void FetchPagination()
        {
            var query = HttpContext.Request.Query;
            Pagination = new Pagination();
            if(query.ContainsKey("pageNumber"))
            {
                Pagination.PageNumber = int.Parse(query["pageNumber"]!);
                if(query.ContainsKey("pageSize"))
                {
                    Pagination.PageSize = int.Parse(query["pageSize"]!);
                }
            }
        }
    }
}

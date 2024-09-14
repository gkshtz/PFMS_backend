using Microsoft.AspNetCore.Mvc;
using PFMS.Utils.Request_Data;

namespace PFMS.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public Filter? Filter { get; set; }
        public Sort? Sort { get; set; }
        public Pagination Pagination { get; set; }

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
                    Sort.IsAscending = bool.Parse(query["isAscending"]);
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;

namespace PFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringTransactionsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] RecurringTransactionRequestModel recurringTransactionModel)
        {
            
        }
    }
}

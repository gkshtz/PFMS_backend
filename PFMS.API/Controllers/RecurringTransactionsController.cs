using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringTransactionsController : BaseController
    {
        private readonly IRecurringTransactionsService _recurringTransactionsService;
        private readonly IMapper _mapper;
        public RecurringTransactionsController(IRecurringTransactionsService recurringTransactionsService, IMapper mapper)
        {
            _recurringTransactionsService = recurringTransactionsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] RecurringTransactionRequestModel recurringTransactionModel)
        {
            var recurringTransactionBo = _mapper.Map<RecurringTransactionBo>(recurringTransactionModel);

            await _recurringTransactionsService.AddRecurringTransaction(recurringTransactionBo, UserId);

            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 201,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Created("", response);
        }
    }
}

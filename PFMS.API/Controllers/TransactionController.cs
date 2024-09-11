using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            FetchFilters();
            var userId = Guid.Parse(User.FindFirst("UserId")!.Value);
            List<TransactionBo> transactionsBo = await _transactionService.GetAllTransactionsAsync(userId, Filter);
            List<TransactionResponseModel> transactionsModel = _mapper.Map<List<TransactionResponseModel>>(transactionsBo);
            GenericSuccessResponse<List<TransactionResponseModel>> response = new GenericSuccessResponse<List<TransactionResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = transactionsModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }
    }
}

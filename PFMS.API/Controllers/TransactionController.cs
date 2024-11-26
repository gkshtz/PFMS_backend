using AutoMapper;
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
            FetchParameters();
            List<TransactionBo> transactionsBo = await _transactionService.GetAllTransactionsAsync(UserId, Filter, Sort, Pagination!);
            List<TransactionResponseModel> transactionsModel = _mapper.Map<List<TransactionResponseModel>>(transactionsBo);
            GenericSuccessResponse<List<TransactionResponseModel>> response = new GenericSuccessResponse<List<TransactionResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = transactionsModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transactionBo = await _transactionService.GetByTransactionId(id, UserId);
            var transactionModel = _mapper.Map<TransactionResponseModel>(transactionBo);

            var response = new GenericSuccessResponse<TransactionResponseModel>()
            {
                StatusCode = 200,
                ResponseData = transactionModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] TransactionRequestModel transactionRequest)        
        {
            var transactionBo = _mapper.Map<TransactionBo>(transactionRequest);
            transactionBo = await _transactionService.AddTransaction(transactionBo, UserId);

            var transactionResponse = _mapper.Map<TransactionResponseModel>(transactionBo);

            GenericSuccessResponse<TransactionResponseModel> response = new GenericSuccessResponse<TransactionResponseModel>()
            {
                StatusCode = 200,
                ResponseData = transactionResponse,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = transactionResponse.TransactionId}, response);
        }


        [HttpPatch]
        [Route("{id:Guid}")]
        public async Task<IActionResult> PatchAsync([FromBody] TransactionRequestModel transactionRequest, [FromRoute] Guid id)
        {
            var transactionBo = _mapper.Map<TransactionBo>(transactionRequest);
            await _transactionService.UpdateTransaction(transactionBo, UserId, id);
            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _transactionService.DeleteTransaction(id, UserId);
            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

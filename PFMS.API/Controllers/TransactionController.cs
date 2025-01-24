using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;
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
        private readonly IWebHostEnvironment _webHostingEnvironment;
        public TransactionController(ITransactionService transactionService, IMapper mapper, IWebHostEnvironment webHostingEnvironment)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _webHostingEnvironment = webHostingEnvironment;
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
        public async Task<IActionResult> PostAsync([FromForm] TransactionRequestModel transactionRequest)         
        {
            var transactionBo = _mapper.Map<TransactionBo>(transactionRequest);
            var rootPath = _webHostingEnvironment.ContentRootPath;
            var transactionId = await _transactionService.AddTransaction(transactionBo, UserId, transactionRequest.File, rootPath);
            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = transactionId}, response);
        }


        [HttpPatch]
        [Route("{id:Guid}")]
        public async Task<IActionResult> PatchAsync([FromForm] TransactionRequestModel transactionRequest, [FromRoute] Guid id)
        {
            var transactionBo = _mapper.Map<TransactionBo>(transactionRequest);
            var rootPath = _webHostingEnvironment.ContentRootPath;
            await _transactionService.UpdateTransaction(transactionBo, UserId, id, rootPath);
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

        [HttpGet]
        [Route("total-transaction-amount")]
        public async Task<IActionResult> GetTotalTransactionAmount()
        {
            var totalTransactionAmountBo = await _transactionService.GetTotalTransactionAmountAsync(UserId);
            var totalTransactionAmountModel = _mapper.Map<TotalTransactionAmountModel>(totalTransactionAmountBo);

            var response = new GenericSuccessResponse<TotalTransactionAmountModel>()
            {
                StatusCode = 200,
                ResponseData = totalTransactionAmountModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);

        }

        [HttpGet]
        [Route("{month:int}/{year:int}")]
        public async Task<IActionResult> GetPreviousMonthSummary([FromRoute] int month, [FromRoute] int year)
        {
            var totalMonthlyAmountBo = await _transactionService.GetPreviousMonthSummary(month, year, UserId);
            var monthlySummaryModel = _mapper.Map<MonthlySummaryResponseModel>(totalMonthlyAmountBo);

            var response = new GenericSuccessResponse<MonthlySummaryResponseModel>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = monthlySummaryModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("screenshots/{screenshotId:Guid}")]
        public async Task<IActionResult> DeleteScreenshotAsync([FromRoute] Guid screenshotId)
        {
            await _transactionService.DeleteTransactionScreenshot(screenshotId, UserId);

            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

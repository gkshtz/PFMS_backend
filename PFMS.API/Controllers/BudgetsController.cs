using System.Net;
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
    public class BudgetsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IBudgetsService _budgetService;
        public BudgetsController(IBudgetsService budgetService, IMapper mapper)
        {
            _budgetService = budgetService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] BudgetRequestModel budgetModel)
        {
            var budgetBo = _mapper.Map<BudgetBo>(budgetModel);
            await _budgetService.AddNewBudget(budgetBo, UserId);

            GenericSuccessResponse<bool> response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.Created,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Created(String.Empty, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgetAsync([FromQuery] int month, [FromQuery] int year)
        {
            var budgetBo = await _budgetService.GetBudget(UserId, month, year);
            var budgetModel = _mapper.Map<BudgetResponseModel>(budgetBo);

            var response = new GenericSuccessResponse<BudgetResponseModel>()
            {
                StatusCode = 200,
                ResponseData = budgetModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBudgetAsync([FromBody] BudgetRequestModel budgetModel, [FromRoute] Guid id)
        {
            var budgetBo = _mapper.Map<BudgetBo>(budgetModel);
            await _budgetService.UpdateBudget(budgetBo, UserId, id);

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

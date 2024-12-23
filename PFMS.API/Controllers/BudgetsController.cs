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
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

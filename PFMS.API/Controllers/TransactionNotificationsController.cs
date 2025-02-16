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
    public class TransactionNotificationsController: BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITransactionNotificationsService _notificationsService;
        public TransactionNotificationsController(ITransactionNotificationsService notificationsService, IMapper mapper)
        {
            _notificationsService = notificationsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] TransactionNotificationRequestModel notificationModel)
        {
            var notificationBo = _mapper.Map<TransactionNotificationBo>(notificationModel);
            await _notificationsService.AddTransactionNotification(notificationBo, UserId);

            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Created("", response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<TransactionNotificationBo> notificationBos = await _notificationsService.GetAllNotificationsOfUser(UserId);
            var notificationModels = _mapper.Map<List<TransactionNotificationResponseModel>>(notificationBos);

            var response = new GenericSuccessResponse<List<TransactionNotificationResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = notificationModels,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

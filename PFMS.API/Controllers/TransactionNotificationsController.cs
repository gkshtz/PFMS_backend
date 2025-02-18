using System.Net;
using System.Security.Cryptography.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            TransactionNotificationBo notificationBo = await _notificationsService.GetNotificationById(id, UserId);
            var notificationModel = _mapper.Map<TransactionNotificationResponseModel>(notificationBo);

            var response = new GenericSuccessResponse<TransactionNotificationResponseModel>()
            {
                StatusCode = 200,
                ResponseData = notificationModel,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpPatch]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromBody] TransactionNotificationRequestModel notificationModel, [FromRoute] Guid id)
        {
            var notificationBo = _mapper.Map<TransactionNotificationBo>(notificationModel);
            await _notificationsService.UpdateTransactionNotificationAsync(notificationBo, id, UserId);

            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _notificationsService.DeleteTransactionNotificationAsync(id, UserId);

            var response = new GenericSuccessResponse<bool>()
            {
                StatusCode = 200,
                ResponseData = true,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")!.Value);
            List<TransactionCategoryBo> categoryBos = await _categoryService.GetAllCategories(userId);
            List<TransactionCategoryResponseModel> categories = _mapper.Map<List<TransactionCategoryResponseModel>>(categoryBos);
            var response = new GenericSuccessResponse<List<TransactionCategoryResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = categories,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

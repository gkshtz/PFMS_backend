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
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            List<TransactionCategoryBo> categoryBos = await _categoryService.GetAllCategories(UserId);
            List<TransactionCategoryResponseModel> categoryResponse = _mapper.Map<List<TransactionCategoryResponseModel>>(categoryBos);
            var response = new GenericSuccessResponse<List<TransactionCategoryResponseModel>>()
            {
                StatusCode = 200,
                ResponseData = categoryResponse,
                ResponseMessage = ResponseMessage.Success.ToString()
            };
            return Ok(response);
        }
    }
}

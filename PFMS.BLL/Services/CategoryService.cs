using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Identity.Client;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<TransactionCategoryBo>> GetAllCategories(Guid userId)
        {
            List<TransactionCategoryDto> categoryDtos = await _categoryRepository.GetAllCategories(userId);
            return _mapper.Map<List<TransactionCategoryBo>>(categoryDtos);
        }
    }
}

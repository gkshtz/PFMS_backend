using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Identity.Client;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.Interfaces;

namespace PFMS.BLL.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;            
        }
        public async Task<List<TransactionCategoryBo>> GetAllCategories(Guid userId)
        {
            var categoryDtos = await _categoryRepository.GetAllCategories(userId); 
            var categoryBos = _mapper.Map<List<TransactionCategoryBo>>(categoryDtos);
            return categoryBos;
        }
    }
}

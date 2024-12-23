﻿using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;

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
        public async Task<List<TransactionCategoryBo>> GetAllCategories(Guid userId, TransactionType transactionType)
        {
            List<TransactionCategoryDto> categoryDtos = await _categoryRepository.GetAllCategories(userId, transactionType);
            return _mapper.Map<List<TransactionCategoryBo>>(categoryDtos);
        }

        public async Task AddCategory(TransactionCategoryBo categoryBo, Guid userId)
        {
            categoryBo.CategoryId = Guid.NewGuid();
            var categoryDto = _mapper.Map<TransactionCategoryDto>(categoryBo);
            await _categoryRepository.AddCategory(categoryDto);

            var categoryToUserBo = new CategoryToUserBo()
            {
                CategoryId = categoryBo.CategoryId,
                UserId = userId
            };
            var categoryToUerDto = _mapper.Map<CategoryToUserDto>(categoryToUserBo);
            await _categoryRepository.AddCategoryToUser(categoryToUerDto);
        }

        public async Task DeleteCategory(Guid categoryId, Guid userId)
        {
            var userIdOfCategory = await _categoryRepository.GetUserIdByCategoryId(categoryId);
            if(userIdOfCategory == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.CategoryNotFound);
            }
            if(userIdOfCategory!=userId)
            {
                throw new ForbiddenException(ErrorMessages.CannotDeleteCategory);
            }

            await _categoryRepository.DeleteCategoryToUser(categoryId);
            await _categoryRepository.DeleteCategory(categoryId);
        }
    }
}

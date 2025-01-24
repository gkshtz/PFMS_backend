using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<TransactionCategoryBo>> GetAllCategories(Guid userId, TransactionType transactionType)
        {
            List<TransactionCategoryDto> categoryDtos = await _unitOfWork.CategoriesRepository.GetAllCategories(userId, transactionType);
            return _mapper.Map<List<TransactionCategoryBo>>(categoryDtos);
        }

        public async Task AddCategory(TransactionCategoryBo categoryBo, Guid userId)
        {
            categoryBo.Id = Guid.NewGuid();
            var categoryDto = _mapper.Map<TransactionCategoryDto>(categoryBo);
            await _unitOfWork.CategoriesRepository.AddAsync(categoryDto);

            var categoryToUserBo = new CategoryToUserBo()
            {
                CategoryId = categoryBo.Id,
                UserId = userId
            };
            var categoryToUerDto = _mapper.Map<CategoryToUserDto>(categoryToUserBo);
            await _unitOfWork.CategoriesRepository.AddCategoryToUser(categoryToUerDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task DeleteCategory(Guid categoryId, Guid userId)
        {
            var userIdOfCategory = await _unitOfWork.CategoriesRepository.GetUserIdByCategoryId(categoryId);
            if(userIdOfCategory == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.CategoryNotFound);
            }
            if(userIdOfCategory!=userId)
            {
                throw new ForbiddenException(ErrorMessages.CannotDeleteCategory);
            }

            await _unitOfWork.CategoriesRepository.DeleteCategoryToUser(categoryId);
            await _unitOfWork.CategoriesRepository.DeleteAsync(categoryId);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }
    }
}

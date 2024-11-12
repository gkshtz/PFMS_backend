using PFMS.DAL.DTOs;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId, TransactionType transactionType);

        public Task AddCategory(TransactionCategoryDto categoryDto);

        public Task AddCategoryToUser(CategoryToUserDto categoryToUserDto);

        public Task<bool> DeleteCategory(Guid categoryId);

        public Task<bool> DeleteCategoryToUser(Guid categoryId);

        public Task<Guid?> GetUserIdByCategoryId(Guid categoryId);
    }
}

using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId);

        public Task AddCategory(TransactionCategoryDto categoryDto);

        public Task AddCategoryToUser(CategoryToUserDto categoryToUserDto);

        public Task<bool> DeleteCategory(Guid categoryId);

        public Task<bool> DeleteCategoryToUser(Guid categoryId);

        public Task<Guid?> GetUserIdByCategoryId(Guid categoryId);
    }
}

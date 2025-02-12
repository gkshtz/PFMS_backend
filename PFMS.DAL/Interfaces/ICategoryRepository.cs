using PFMS.DAL.DTOs;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Interfaces
{
    public interface ICategoryRepository<Dto>: IGenericRepository<Dto>
        where Dto: TransactionCategoryDto
    {
        public Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId, TransactionType transactionType);

        public Task AddCategoryToUser(CategoryToUserDto categoryToUserDto);

        public Task<bool> DeleteCategoryToUser(Guid categoryId);

        public Task<Guid?> GetUserIdByCategoryId(Guid categoryId);

        public Task<IEnumerable<Guid>> DeleteAllCategoryToUserByUserId(Guid userId);

        public Task DeleteAllCategoriesByCategoryIds(IEnumerable<Guid> categoryIds);
    }
}

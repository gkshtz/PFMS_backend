using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.DAL.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;            
        }
        public async Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId, TransactionType transactionType)
        {
            List<TransactionCategory> categories = await _appDbContext.CategoryToUser.Include(x=>x.Category).Where(x => (x.UserId == null || x.UserId == userId) && (x.Category!.TransactionType == transactionType.ToString())).Select(x=>x.Category!).ToListAsync();
            return _mapper.Map<List<TransactionCategoryDto>>(categories);
        }

        public async Task AddCategory(TransactionCategoryDto categoryDto)
        {
            var category = _mapper.Map<TransactionCategory>(categoryDto);
            await _appDbContext.TransactionCategories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task AddCategoryToUser(CategoryToUserDto categoryToUserDto)
        {
            var categoryToUser = _mapper.Map<CategoryToUser>(categoryToUserDto);
            await _appDbContext.CategoryToUser.AddAsync(categoryToUser);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var category = await _appDbContext.TransactionCategories.FindAsync(categoryId);
            if(category == null)
            {
                return false;
            }
            _appDbContext.TransactionCategories.Remove(category);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryToUser(Guid categoryId)
        {
            var categoryToUser = await _appDbContext.CategoryToUser.FindAsync(categoryId);
            if(categoryToUser == null)
            {
                return false;
            }
            _appDbContext.CategoryToUser.Remove(categoryToUser);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Guid?> GetUserIdByCategoryId(Guid categoryId)
        {
            var categoryToUser = await _appDbContext.CategoryToUser.Where(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
            if(categoryToUser == null)
            {
                return null;
            }
            return categoryToUser.UserId == null ? Guid.Empty : categoryToUser.UserId;
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class CategoryRepository<Dto, Entity>: GenericRepository<Dto, Entity>, ICategoryRepository<Dto>
        where Dto: TransactionCategoryDto
        where Entity: TransactionCategory
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;            
        }
        public async Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId, TransactionType transactionType)
        {
            List<TransactionCategory> categories = await _appDbContext.CategoryToUser.Include(x=>x.Category).Where(x => (x.UserId == null || x.UserId == userId) && (x.Category!.TransactionType == transactionType.ToString())).Select(x=>x.Category!).ToListAsync();
            return _mapper.Map<List<TransactionCategoryDto>>(categories);
        }

        public async Task AddCategoryToUser(CategoryToUserDto categoryToUserDto)
        {
            var categoryToUser = _mapper.Map<CategoryToUser>(categoryToUserDto);
            await _appDbContext.CategoryToUser.AddAsync(categoryToUser);
        }

        public async Task<bool> DeleteCategoryToUser(Guid categoryId)
        {
            var categoryToUser = await _appDbContext.CategoryToUser.FindAsync(categoryId);
            if(categoryToUser == null)
            {
                return false;
            }
            _appDbContext.CategoryToUser.Remove(categoryToUser);
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

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

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
        public async Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId)
        {
            List<TransactionCategory> categories = await _appDbContext.CategoryToUser.Include(x=>x.Category).Where(x => x.UserId == null || x.UserId == userId).Select(x=>x.Category!).ToListAsync();
            return _mapper.Map<List<TransactionCategoryDto>>(categories);
        }
    }
}

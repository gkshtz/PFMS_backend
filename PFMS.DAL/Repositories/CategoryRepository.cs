using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<TransactionCategoryDto>> GetAllCategories(Guid userId)
        {
            var categories = await _appDbContext.TransactionCategories.Where(x => x.UserId == null || x.UserId == userId).ToListAsync();
            List<TransactionCategoryDto> categoryDtos = _mapper.Map<List<TransactionCategoryDto>>(categories);
            return categoryDtos;
        }
    }
}

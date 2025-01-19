using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class GenericRepository<Dto, Entity>
        where Dto: class, IIdentifiable
        where Entity: class, IIdentifiable
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public GenericRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        
        public async Task AddAsync(Dto dto)
        {
            var entity = _mapper.Map<Entity>(dto);
            await _appDbContext.Set<Entity>().AddAsync(entity);
        }

        public async Task<List<Dto>> GetAllAsync()
        {
            var entities = await _appDbContext.Set<Entity>().ToListAsync();
            return _mapper.Map<List<Dto>>(entities);
        }

        public async Task<Dto> GetByIdAsync(Guid id)
        {
            var entity = await _appDbContext.Set<Entity>().FindAsync(id);
            return _mapper.Map<Dto>(entity);
        }

        
        public async Task<bool> UpdateAsync(Dto dto)
        {
            Entity? entity = await _appDbContext.Set<Entity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);
            if(entity == null)
            {
                return false;
            }

            entity = _mapper.Map<Entity>(dto);
            _appDbContext.Set<Entity>().Update(entity);
            return true;
        }
        
        public async Task DeleteAsync(Guid id)
        {
            Entity? entity = await _appDbContext.Set<Entity>().FirstOrDefaultAsync(x => x.Id == id);
            if(entity != null)
            {
                _appDbContext.Set<Entity>().Remove(entity);
            }
        }
    }
}

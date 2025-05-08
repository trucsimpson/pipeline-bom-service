using AutoMapper;
using BOMService.Domain.Repositories;
using BOMService.Infrastructure.Persistence.EFModels;
using Microsoft.EntityFrameworkCore;

namespace BOMService.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TModel> : IBaseRepository<TModel>
        where TEntity : class
        where TModel : class
    {
        protected readonly AppDbContext _dbContext;
        private DbSet<TEntity> _dbSet;
        private readonly IMapper _mapper;

        public BaseRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _mapper = mapper;
        }

        public async Task<List<TModel>> GetAllAsync(bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            var entities = await query.ToListAsync();
            return _mapper.Map<List<TModel>>(entities);
        }
    }
}

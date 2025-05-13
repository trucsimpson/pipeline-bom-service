using AutoMapper;
using AutoMapper.QueryableExtensions;
using BOMService.Domain.Repositories;
using BOMService.Infrastructure.Persistence.EFModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BOMService.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel>
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

        public async Task<List<TModel>> GetAsync(
            Expression<Func<TEntity, TModel>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeString = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (disableTracking)
                query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (selector != null)
                return await query.Select(selector).ToListAsync();

            return await query
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

    }
}

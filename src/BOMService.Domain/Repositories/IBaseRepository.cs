using System.Linq.Expressions;

namespace BOMService.Domain.Repositories
{
    public interface IBaseRepository<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        Task<List<TModel>> GetAllAsync(bool disableTracking = true);

        Task<List<TModel>> GetAsync(
            Expression<Func<TEntity, TModel>> selector = null,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeString = null,
            bool disableTracking = true);
    }
}

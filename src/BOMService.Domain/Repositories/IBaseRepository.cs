using System.Linq.Expressions;

namespace BOMService.Domain.Repositories
{
    public interface IBaseRepository<TModel>
    {
        Task<List<TModel>> GetAllAsync(bool disableTracking = true);
    }
}

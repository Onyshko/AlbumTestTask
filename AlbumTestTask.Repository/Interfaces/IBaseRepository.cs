using System.Linq.Expressions;

namespace AlbumTestTask.Repository.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(int id);

        Task<int> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(int id);

        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
    }
}

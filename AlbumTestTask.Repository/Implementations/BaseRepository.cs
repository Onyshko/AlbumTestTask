using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using AlbumTestTask.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlbumTestTask.Repository.Implementations
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingEntity = await GetAsync(id);

            if (existingEntity == null)
            {
                return false;
            }

            _context.Entry(existingEntity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public virtual Task<TEntity> GetAsync(int id)
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var existingEntity = await GetAsync(entity.Id);

            if (existingEntity == null)
            {
                return existingEntity;
            }

            UpdateRelatedEntities(existingEntity, entity);
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return existingEntity;
        }

        protected virtual void UpdateRelatedEntities(TEntity existingEntity, TEntity entity)
        {
        }
    }
}

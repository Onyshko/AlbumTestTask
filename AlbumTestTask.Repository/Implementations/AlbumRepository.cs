using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AlbumTestTask.Repository.Implementations
{
    public class AlbumRepository : BaseRepository<Album>
    {
        public AlbumRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async override Task<List<Album>> GetAllAsync()
        {
            return await _context.Set<Album>().Include(x => x.Photos).ThenInclude(p => p.Likes).ToListAsync();
        }

        public async override Task<Album> GetAsync(int id)
        {
            return await _context.Set<Album>().Include(x => x.Photos).ThenInclude(p => p.Likes).FirstOrDefaultAsync(entity => entity.Id == id);
        }

        protected override void UpdateRelatedEntities(Album existingEntity, Album entity)
        {
            UpdateLikes(existingEntity, entity);
        }

        private void UpdateLikes(Album existingEntity, Album entity)
        {
            var toDelete = existingEntity.Photos.Where(x => !entity.Photos.Any(y => y.Id == x.Id)).ToList();
            foreach (var item in toDelete)
            {
                _context.Entry(item).State = EntityState.Deleted;
                existingEntity.Photos.Remove(item);
            }

            var toAdd = entity.Photos.Where(x => !existingEntity.Photos.Any(y => y.Id == x.Id)).ToList();
            foreach (var item in toAdd)
            {
                _context.Entry(item).State = EntityState.Added;
                existingEntity.Photos.Add(item);
            }
        }
    }
}

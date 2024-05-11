using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlbumTestTask.Repository.Implementations
{
    public class PhotoRepository : BaseRepository<Photo>
    {
        public PhotoRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public override Task<List<Photo>> GetAllAsync()
        {
            return GetAllAsync(x => x.Likes);
        }

        protected override void UpdateRelatedEntities(Photo existingEntity, Photo entity)
        {
            UpdateLikes(existingEntity, entity);
        }

        private void UpdateLikes(Photo existingEntity, Photo entity)
        {
            var toDelete = existingEntity.Likes.Where(x => !entity.Likes.Any(y => y.Id == x.Id)).ToList();
            foreach (var item in toDelete)
            {
                _context.Entry(item).State = EntityState.Deleted;
                existingEntity.Likes.Remove(item);
            }

            var toAdd = entity.Likes.Where(x => !existingEntity.Likes.Any(y => y.Id == x.Id)).ToList();
            foreach (var item in toAdd)
            {
                _context.Entry(item).State = EntityState.Added;
                existingEntity.Likes.Add(item);
            }
        }
    }
}

using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using AlbumTestTask.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlbumTestTask.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().Include(x => x.Albums).ToListAsync();
        }

        public async Task<ApplicationUser> GetAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            var existingEntity = await GetAsync(user.Id);
            if (existingEntity == null)
            {
                return existingEntity;
            }

            UpdateRelatedEntities(existingEntity, user);
            _context.Entry(existingEntity).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        private void UpdateRelatedEntities(ApplicationUser existingEntity, ApplicationUser user)
        {
            var toDeleteAlbums = existingEntity.Albums.Where(x => !user.Albums.Any(y => y.Id == x.Id)).ToList();
            foreach (var album in toDeleteAlbums)
            {
                existingEntity.Albums.Remove(album);
            }

            var toAddAlbums = user.Albums.Where(x => !existingEntity.Albums.Any(y => y.Id == x.Id)).ToList();
            foreach (var album in toAddAlbums)
            {
                existingEntity.Albums.Add(album);
            }
        }
    }
}

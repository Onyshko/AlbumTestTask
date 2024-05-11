using AlbumTestTask.Domain.Entities;

namespace AlbumTestTask.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetAsync(string id);

        Task<List<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
    }
}

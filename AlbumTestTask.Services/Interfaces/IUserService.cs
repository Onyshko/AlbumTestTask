using AlbumTestTask.Services.Models;

namespace AlbumTestTask.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetAsync(string id);
    }
}

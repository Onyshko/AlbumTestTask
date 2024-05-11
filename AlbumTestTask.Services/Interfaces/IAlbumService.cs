using AlbumTestTask.Services.Models;
using System.Security.Claims;

namespace AlbumTestTask.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IList<AlbumModel>> GetAlbumsForUserAsync(ClaimsPrincipal user);
    }
}

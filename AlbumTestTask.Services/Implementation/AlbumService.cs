using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AlbumTestTask.Services.Implementation
{
    public class AlbumService : IAlbumService
    {
        private readonly IBaseCrudService<AlbumModel> _albumService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumService(IBaseCrudService<AlbumModel> albumService, UserManager<ApplicationUser> userManager)
        {
            _albumService = albumService;
            _userManager = userManager;
        }

        public async Task<IList<AlbumModel>> GetAlbumsForUserAsync(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            var albums = (await _albumService.GetAllAsync()).Where(x => x.UserId == userId).ToList();
            return albums;
        }
    }
}

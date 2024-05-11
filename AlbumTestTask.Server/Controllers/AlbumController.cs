using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlbumTestTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IBaseCrudService<AlbumModel> _albumCrudService;
        private readonly IAlbumService _albumService;

        public AlbumController(IBaseCrudService<AlbumModel> albumCrudService, IAlbumService albumService)
        {
            _albumCrudService = albumCrudService;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAlbums()
        {
            try
            {
                var albums = await _albumCrudService.GetAllAsync();
                if (albums.Count == 0)
                {
                    return NotFound("Albums not found");
                }

                return Ok(albums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetAlbumsForUserAlbums()
        {
            try
            {
                var albums = await _albumService.GetAlbumsForUserAsync(User);
                if (albums.Count == 0)
                {
                    return NotFound("Albums not found");
                }

                return Ok(albums);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlbum(int id)
        {
            try
            {
                var album = await _albumCrudService.GetAsync(id);
                if (album is null)
                {
                    return NotFound("Album not found");
                }

                return Ok(album);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum(AlbumModel model)
        {
            await _albumCrudService.AddAsync(model);

            return Ok("Album added");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFilm(AlbumModel model)
        {
            await _albumCrudService.UpdateAsync(model);

            return Ok("Album updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            await _albumCrudService.DeleteAsync(id);

            return Ok("Album deleted");
        }
    }
}

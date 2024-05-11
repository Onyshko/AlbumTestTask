using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlbumTestTask.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IBaseCrudService<PhotoModel> _photoService;

        public PhotoController(IBaseCrudService<PhotoModel> photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPhotos()
        {
            try
            {
                var photos = await _photoService.GetAllAsync();
                if (photos.Count == 0)
                {
                    return NotFound("Photos not found");
                }

                return Ok(photos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            try
            {
                var photo = await _photoService.GetAsync(id);
                if (photo is null)
                {
                    return NotFound("Photo not found");
                }

                return Ok(photo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(PhotoModel model)
        {
            await _photoService.AddAsync(model);

            return Ok("Photo added");
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePhoto(PhotoModel model)
        {
            await _photoService.UpdateAsync(model);

            return Ok("Photo updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            await _photoService.DeleteAsync(id);

            return Ok("Photo deleted");
        }
    }
}

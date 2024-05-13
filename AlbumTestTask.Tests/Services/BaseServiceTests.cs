using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Interfaces;
using AlbumTestTask.Services.Implementation;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AlbumTestTask.Tests.Services
{
    public class PhotoCrudServiceTests
    {
        private readonly IBaseCrudService<PhotoModel> _service;
        private readonly Mock<IBaseRepository<Photo>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ServiceProvider _serviceProvider;

        public PhotoCrudServiceTests()
        {
            var services = new ServiceCollection();
            _mockRepo = new Mock<IBaseRepository<Photo>>();
            _mockMapper = new Mock<IMapper>();

            services.AddSingleton(_mockRepo.Object);
            services.AddSingleton(_mockMapper.Object);
            services.AddScoped<IBaseCrudService<PhotoModel>, BaseCrudService<Photo, PhotoModel>>();

            _serviceProvider = services.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<IBaseCrudService<PhotoModel>>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPhotos_MappedToModel()
        {
            var photos = new List<Photo> { new Photo { Id = 1, AlbumId = 1 } };
            var photoModels = new List<PhotoModel> { new PhotoModel { Id = 1, AlbumId = 1 } };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(photos);
            _mockMapper.Setup(m => m.Map<List<PhotoModel>>(photos)).Returns(photoModels);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            _mockMapper.Verify(m => m.Map<List<PhotoModel>>(photos), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsPhotoById_MappedToModel()
        {
            var photo = new Photo { Id = 1, AlbumId = 1 };
            var photoModel = new PhotoModel { Id = 1, AlbumId = 1 };

            _mockRepo.Setup(r => r.GetAsync(1)).ReturnsAsync(photo);
            _mockMapper.Setup(m => m.Map<PhotoModel>(photo)).Returns(photoModel);

            var result = await _service.GetAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockMapper.Verify(m => m.Map<PhotoModel>(photo), Times.Once);
        }

        [Fact]
        public async Task AddAsync_AddsPhotoAndReturnsId()
        {
            var photoModel = new PhotoModel { Id = 1, AlbumId = 1 };
            var photo = new Photo { Id = 1, AlbumId = 1 };

            _mockMapper.Setup(m => m.Map<Photo>(photoModel)).Returns(photo);
            _mockRepo.Setup(r => r.AddAsync(photo)).ReturnsAsync(photo.Id);

            var result = await _service.AddAsync(photoModel);

            Assert.Equal(1, result);
            _mockRepo.Verify(r => r.AddAsync(photo), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPhotoAndReturnsUpdatedModel()
        {
            var photoModel = new PhotoModel { Id = 1, AlbumId = 1 };
            var photo = new Photo { Id = 1, AlbumId = 1 };

            _mockMapper.Setup(m => m.Map<Photo>(photoModel)).Returns(photo);
            _mockRepo.Setup(r => r.UpdateAsync(photo)).ReturnsAsync(photo);
            _mockMapper.Setup(m => m.Map<PhotoModel>(photo)).Returns(photoModel);

            var result = await _service.UpdateAsync(photoModel);

            Assert.NotNull(result);
            _mockRepo.Verify(r => r.UpdateAsync(photo), Times.Once);
            _mockMapper.Verify(m => m.Map<PhotoModel>(photo), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesPhotoAndReturnsTrue()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}

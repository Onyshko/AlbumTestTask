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
    public class UserServiceTests
    {
        private readonly IUserService _service;
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ServiceProvider _serviceProvider;

        public UserServiceTests()
        {
            var services = new ServiceCollection();
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            services.AddSingleton(_mockRepo.Object);
            services.AddSingleton(_mockMapper.Object);
            services.AddScoped<IUserService, UserService>();

            _serviceProvider = services.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<IUserService>();
        }

        [Fact]
        public async Task GetAsync_ReturnsUserModel_MappedFromUserEntity()
        {
            var userId = "user1";
            var userEntity = new ApplicationUser { Id = userId, Email = "user1@example.com" };
            var userModel = new UserModel { Id = userId, Email = "user1@example.com" };

            _mockRepo.Setup(r => r.GetAsync(userId)).ReturnsAsync(userEntity);
            _mockMapper.Setup(m => m.Map<UserModel>(userEntity)).Returns(userModel);

            var result = await _service.GetAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("user1@example.com", result.Email);
            _mockMapper.Verify(m => m.Map<UserModel>(userEntity), Times.Once);
        }
    }
}

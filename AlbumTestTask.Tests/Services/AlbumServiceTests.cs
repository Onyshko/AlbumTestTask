using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Implementation;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace AlbumTestTask.Tests.Services
{
    public class AlbumServiceTests
    {
        private readonly IAlbumService _service;
        private readonly Mock<IBaseCrudService<AlbumModel>> _mockAlbumService;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly ServiceProvider _serviceProvider;

        public AlbumServiceTests()
        {
            var services = new ServiceCollection();
            _mockAlbumService = new Mock<IBaseCrudService<AlbumModel>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            services.AddSingleton(_mockAlbumService.Object);
            services.AddSingleton(_mockUserManager.Object);
            services.AddScoped<IAlbumService, AlbumService>();

            _serviceProvider = services.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<IAlbumService>();
        }

        [Fact]
        public async Task GetAlbumsForUserAsync_ReturnsAlbumsForSpecificUser()
        {
            var userId = "user123";
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            var albums = new List<AlbumModel>
            {
                new AlbumModel { Id = 1, UserId = userId, Name = "Album 1" },
                new AlbumModel { Id = 2, UserId = "user456", Name = "Album 2" }
            };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mockAlbumService.Setup(s => s.GetAllAsync()).ReturnsAsync(albums);

            var result = await _service.GetAlbumsForUserAsync(claims);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Album 1", result.First().Name);
            Assert.Equal(userId, result.First().UserId);
        }
    }
}

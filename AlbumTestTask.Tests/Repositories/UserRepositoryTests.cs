using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using AlbumTestTask.Repository.Implementations;
using AlbumTestTask.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlbumTestTask.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly ServiceProvider _serviceProvider;

        public UserRepositoryTests()
        {
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            _serviceProvider = services.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers_WithAlbums()
        {
            var newUser = new ApplicationUser
            {
                UserName = "testuser1",
                Albums = new List<Album>
                {
                    new Album { Name = "Test Album 1" }
                }
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var result = await _userRepository.GetAllAsync();

            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Single(result.First().Albums);
        }

        [Fact]
        public async Task GetAsync_ReturnsUserById()
        {
            var newUser = new ApplicationUser { UserName = "testuser2" };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            var userId = newUser.Id;

            var result = await _userRepository.GetAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUserAndRelatedEntities()
        {
            var newUser = new ApplicationUser
            {
                UserName = "testuser3",
                Albums = new List<Album>
                {
                    new Album { Name = "Old Album" }
                }
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            newUser.Albums.Add(new Album { Name = "New Album" });

            var updatedUser = await _userRepository.UpdateAsync(newUser);

            Assert.NotNull(updatedUser);
            Assert.Equal(2, updatedUser.Albums.Count);
            Assert.Contains(updatedUser.Albums, a => a.Name == "New Album");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _serviceProvider.Dispose();
        }
    }
}

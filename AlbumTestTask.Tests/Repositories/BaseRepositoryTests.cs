using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using AlbumTestTask.Repository.Implementations;
using AlbumTestTask.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AlbumTestTask.Tests.Repositories
{
    public class BaseRepositoryTests
    {
        private readonly IBaseRepository<Album> _repository;
        private readonly ApplicationDbContext _context;
        private readonly ServiceProvider _serviceProvider;

        public BaseRepositoryTests()
        {
            var services = new ServiceCollection();
            var databaseName = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseRepository<Album>), typeof(AlbumRepository));

            _serviceProvider = services.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            _repository = _serviceProvider.GetRequiredService<IBaseRepository<Album>>();

            _context.Database.EnsureCreated();

            _context.Albums.Add(new Album
            {
                Id = 1,
                Name = "Test Album",
                UserId = "User1",
                Photos = new List<Photo> {
                    new Photo {
                        Id = 11,
                        Data = Encoding.UTF8.GetBytes("Hello world!"),
                        ContentType = "jpeg",
                        LikeCounter = 0,
                        DislikeCounter = 0,
                        AlbumId = 1,
                        Likes = new List<Like> {
                            new Like {
                                Id = 111,
                                UserId = "User1",
                                PhotoId = 11,
                                ActionType = "like"
                            }
                        }
                    }
                }
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAlbums_WithPhotosAndLikes()
        {
            var result = await _repository.GetAllAsync();

            Assert.Single(result);
            var album = result.First();
            Assert.Single(album.Photos);
            var photo = album.Photos.First();
            Assert.Single(photo.Likes);
        }

        [Fact]
        public async Task GetAsync_ReturnsAlbumById_WithPhotosAndLikes()
        {
            var result = await _repository.GetAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Single(result.Photos);
            Assert.Single(result.Photos.First().Likes);
        }

        [Fact]
        public async Task AddAndGetAlbum_AreEqual()
        {
            var newAlbum = new Album
            {
                Name = "Test Album",
                UserId = "User1",
                Photos = new List<Photo> {
                    new Photo {
                        Data = Encoding.UTF8.GetBytes("Hello world!"),
                        ContentType = "jpeg",
                        LikeCounter = 0,
                        DislikeCounter = 0,
                        Likes = new List<Like> {
                            new Like {
                                UserId = "User1",
                                ActionType = "like"
                            }
                        }
                    }
                }
            };

            int newAlbumId = await _repository.AddAsync(newAlbum);

            var retrievedAlbum = await _repository.GetAsync(newAlbumId);

            Assert.NotNull(retrievedAlbum);
            Assert.Equal(newAlbumId, retrievedAlbum.Id);
            Assert.Equal(newAlbum.Name, retrievedAlbum.Name);
            Assert.Equal(newAlbum.UserId, retrievedAlbum.UserId);
        }

        [Fact]
        public async Task GetAllAsync_WithIncludes_ReturnsAlbumsWithPhotos()
        {
            var album = new Album
            {
                Name = "Test Album",
                UserId = "User2",
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        Data = Encoding.UTF8.GetBytes("Sample Image"),
                        ContentType = "image/jpeg"
                    }
                }
            };

            await _repository.AddAsync(album);

            var albums = await _repository.GetAllAsync(a => a.Photos);

            Assert.NotEmpty(albums);
            var retrievedAlbum = albums.First();
            Assert.NotEmpty(retrievedAlbum.Photos);
            var photo = retrievedAlbum.Photos.First();
            Assert.Equal(retrievedAlbum.Id, photo.AlbumId);
        }

        [Fact]
        public async Task UpdateAlbum_CheckPropertiesConsistency()
        {
            var album = await _repository.GetAllAsync();
            var firstAlbum = album.First();

            firstAlbum.Name = "Updated Album";

            await _repository.UpdateAsync(firstAlbum);

            var updatedAlbum = await _repository.GetAsync(firstAlbum.Id);

            Assert.NotNull(updatedAlbum);
            Assert.Equal("Updated Album", updatedAlbum.Name);
            Assert.Equal(firstAlbum.Id, updatedAlbum.Id);
        }

        [Fact]
        public async Task DeleteAlbum_CheckDeletion()
        {
            var newAlbum = new Album
            {
                Name = "Album to Delete",
                UserId = "User1"
            };

            var newAlbumId = await _repository.AddAsync(newAlbum);

            var deleteResult = await _repository.DeleteAsync(newAlbumId);
            Assert.True(deleteResult);

            var retrievedAlbum = await _repository.GetAsync(newAlbumId);
            Assert.Null(retrievedAlbum);
        }
    }
}

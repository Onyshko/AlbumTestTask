using AlbumTestTask.Repository.Interfaces;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Models;
using AutoMapper;

namespace AlbumTestTask.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserModel> GetAsync(string id)
        {
            var user = await _repo.GetAsync(id);
            return _mapper.Map<UserModel>(user);
        }
    }
}

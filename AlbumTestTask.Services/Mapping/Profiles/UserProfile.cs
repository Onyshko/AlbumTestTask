using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Models;
using AutoMapper;

namespace AlbumTestTask.Services.Mapping.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserModel>().ReverseMap();
        }
    }
}

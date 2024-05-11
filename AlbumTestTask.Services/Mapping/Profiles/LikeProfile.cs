using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Models;
using AutoMapper;

namespace AlbumTestTask.Services.Mapping.Profiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<Like, LikeModel>().ReverseMap();
        }
    }
}

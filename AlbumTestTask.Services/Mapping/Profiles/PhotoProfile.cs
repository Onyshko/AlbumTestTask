using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Models;
using AutoMapper;

namespace AlbumTestTask.Services.Mapping.Profiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoModel>().ReverseMap();
        }
    }
}

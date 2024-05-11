using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Services.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AlbumTestTask.Services.Mapping.Profiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<Album, AlbumModel>().ReverseMap();
        }
    }
}

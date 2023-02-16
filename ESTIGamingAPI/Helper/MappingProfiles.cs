using AutoMapper;
using ESTIGamingAPI.Dto;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Game, GameDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Platform, PlatformDto>();
            CreateMap<User, UserDto>();
        }
    }
}

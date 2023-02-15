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
        }
    }
}

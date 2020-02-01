using System.Linq;
using AutoMapper;
using MemeRing.Dtos;
using MemeRing.Models;
using MemeRing.Services.Interfaces;

namespace MemeRing.Helpers
{
    /// <summary>
    /// Represents a AutoMapper. 
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForRegisterDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserResponseDto, User>();
        }
    }
}

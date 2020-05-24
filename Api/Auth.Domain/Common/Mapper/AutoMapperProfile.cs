using Auth.Domain.Services.User.Dto;
using Auth.Domain.Services.User.Entities;
using AutoMapper;

namespace Auth.Domain.Common.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();
        }
    }
}
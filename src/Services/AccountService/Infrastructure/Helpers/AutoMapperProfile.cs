using AccountService.Dtos;
using AccountService.Entities;
using AutoMapper;

namespace AccountService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserCommand>();
            CreateMap<UserCommand, User>();
        }
    }
}
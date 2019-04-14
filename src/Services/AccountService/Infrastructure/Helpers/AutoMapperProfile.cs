using AccountService.Domain.Write.Commands;
using AccountService.Domain.Write.State;
using AutoMapper;

namespace AccountService.Infrastructure.Helpers
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
using AutoMapper;
using IdentityService.Domain.Command;
using IdentityService.Domain.State;

namespace IdentityService.Infrastructure.Helpers
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
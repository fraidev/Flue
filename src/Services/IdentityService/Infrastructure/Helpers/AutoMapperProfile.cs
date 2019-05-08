using AutoMapper;
using IdentityService.Domain.Commands;
using IdentityService.Domain.State;

namespace IdentityService.Infrastructure.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserState, UserCommand>();
            CreateMap<UserCommand, UserState>();
        }
    }
}
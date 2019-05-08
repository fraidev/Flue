using AutoMapper;
using IdentityService.Domain.Write.Commands;
using IdentityService.Domain.Write.State;

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
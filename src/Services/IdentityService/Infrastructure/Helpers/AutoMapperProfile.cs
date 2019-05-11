using AutoMapper;
using IdentityService.Domain.Models;
using IdentityService.Domain.State;

namespace IdentityService.Infrastructure.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserState, Identifier>();
            CreateMap<Identifier, UserState>();
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Mapping;
using IdentityService.Domain.State;

namespace IdentityService.Infrastructure.Persistence.Maps
{
    [ExcludeFromCodeCoverage]
    public class UserStateMap: ClassMap<User>
    {
        public UserStateMap()
        {
            Table("[User]");
            Id(x => x.UserId).GeneratedBy.GuidComb();
            Map(x => x.Username);
            Map(x => x.PasswordHash);
            Map(x => x.PasswordSalt);
            Map(x => x.Role);
            Map(x => x.Token);
        }
    }
}
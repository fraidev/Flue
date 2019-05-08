using FluentNHibernate.Mapping;
using IdentityService.Domain.State;

namespace IdentityService.Infrastructure.Persistence.Maps
{
    public class UserStateMap: ClassMap<UserState>
    {
        public UserStateMap()
        {
            Table("[User]");
            Id(x => x.Id, "UserId").GeneratedBy.GuidComb();
//            Map(x => x.FirstName);
//            Map(x => x.LastName);
            Map(x => x.Username);
            Map(x => x.PasswordHash);
            Map(x => x.PasswordSalt);
            Map(x => x.Role);
            Map(x => x.Token);
            
//            HasManyToMany(x => x.Following)
//                .Cascade.All()
//                //.Inverse()
//                .ParentKeyColumn("FollowingId")
//                .ChildKeyColumn("UserId")
//                .Table("Follows");
        }
    }
}
using AccountService.Domain.Write.State;
using FluentNHibernate.Mapping;

namespace AccountService.Infrastructure.Persistence.Maps
{
    public class UserStateMap: ClassMap<UserState>
    {
        public UserStateMap()
        {
            Table("[User]");
            Id(x => x.Id, "UserId").GeneratedBy.Assigned();
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Username);
            Map(x => x.PasswordHash);
            Map(x => x.PasswordSalt);
            Map(x => x.Role);
            Map(x => x.Token);
            /*HasMany(x => x.Followers)
                .Inverse()
                .KeyColumns.Add("UserId")
                .Cascade.AllDeleteOrphan();*/
            
            HasManyToMany(x => x.Followers)
                .Cascade.All()
                //.Inverse()
                .ParentKeyColumn("FollowersId")
                .ChildKeyColumn("UserId")
                .Table("Follows");
            
            /*HasMany(x => x.Following)
                .Inverse()
                .KeyColumns.Add("UserId")
                .Cascade.AllDeleteOrphan();*/
        }
    }
}
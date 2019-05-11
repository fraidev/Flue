using FeedService.Domain.Write.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Write
{
    public class PersonStateMap: ClassMap<UserState>
    {
        public PersonStateMap()
        {
            Table("[User]");
            Id(x => x.UserId, "UserId").GeneratedBy.GuidComb();
            Map(x => x.IdentifyId).Not.Nullable();
            Map(x => x.Name);
            Map(x => x.Email);
            
            HasManyToMany(x => x.Following)
                .Cascade.All()
                //.Inverse()
                .ParentKeyColumn("FollowingId")
                .ChildKeyColumn("PersonId")
                .Table("Follows");
        }
    }
}
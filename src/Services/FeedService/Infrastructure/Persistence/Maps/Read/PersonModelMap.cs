using FeedService.Domain.Read.Models;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Read
{
    public class PersonModelMap: ClassMap<PersonModel>
    {
        public PersonModelMap()
        {
            ReadOnly();
            Table("[Person]");
            Id(x => x.PersonId).GeneratedBy.GuidComb();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Username);
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
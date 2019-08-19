using FeedService.Domain.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps
{
    public class PersonMap: ClassMap<Person>
    {
        public PersonMap()
        {
            Table("[Person]");
            Id(x => x.PersonId).GeneratedBy.GuidComb();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Username);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Email);
            
            HasManyToMany(x => x.Following)
//                .Cascade.All()
                //.Inverse()
                .ParentKeyColumn("FollowingId")
                .ChildKeyColumn("PersonId")
                .Table("Follows")
                .Cascade.SaveUpdate();
            
            HasManyToMany(x => x.Posts)
                .ParentKeyColumn("PostId")
                .ChildKeyColumn("PersonId")
                .Table("PersonPost")
                .Cascade.SaveUpdate();
        }                
    }
}
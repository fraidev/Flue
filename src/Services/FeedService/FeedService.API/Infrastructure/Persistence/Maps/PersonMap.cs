using FeedService.Domain.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps
{
    public class PersonMap : ClassMap<Person>
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
                .ParentKeyColumn("FollowingId")
                .ChildKeyColumn("PersonId")
                .Table("Follows")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.Followers)
                .ParentKeyColumn("PersonId")
                .ChildKeyColumn("FollowingId")
                .Table("Follows")
                .Cascade.SaveUpdate();

            HasMany(x => x.Posts)
                .KeyColumns.Add("PersonId", x => x.Not.Nullable())
                .Cascade.All();
        }
    }
}
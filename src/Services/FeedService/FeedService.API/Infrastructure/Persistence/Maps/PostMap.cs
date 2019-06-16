using FeedService.Domain.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps
{
    public class PostMap: ClassMap<Post>
    {
        public PostMap()
        {
            Table("Post");
            
            Id(x => x.PostId).GeneratedBy.Assigned();
            
            Map(x => x.Text);
            Map(x => x.Likes);
            Map(x => x.Deleted);
            
            References(x => x.Person, "PersonId");

            HasMany(x => x.Comments)
                .Inverse()
                .KeyColumns.Add("PostId", x => x.Not.Nullable())
                .Cascade.AllDeleteOrphan();
        }
    }
}
using FeedService.Domain.Read.Models;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Read
{
    public class PostModelMap : ClassMap<PostModel>
    {
        public PostModelMap()
        {
            ReadOnly();
            Table("Post");
            Id(x => x.Id).GeneratedBy.Assigned();;
            Map(x => x.Text);
            Map(x => x.UserId);
            Map(x => x.OnFire);

            HasMany(x => x.Comments)
                .Inverse()
                .KeyColumns.Add("PostId", x => x.Not.Nullable())
                .Cascade.AllDeleteOrphan();
        }
    }
}
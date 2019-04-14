using FeedService.Domain.Write.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Write
{
    public class PostStateMap: ClassMap<PostState>
    {
        public PostStateMap()
        {
            Table("Post");
            Id(x => x.Id, "PostId").GeneratedBy.Assigned();;
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
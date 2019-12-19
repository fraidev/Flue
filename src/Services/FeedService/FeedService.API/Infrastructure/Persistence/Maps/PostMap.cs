using System.Diagnostics.CodeAnalysis;
using FeedService.Domain.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps
{
    [ExcludeFromCodeCoverage]
    public class PostMap: ClassMap<Post>
    {
        public PostMap()
        {
            Table("Post");
            
            Id(x => x.PostId).GeneratedBy.Assigned();
            
            Map(x => x.Text);
            Map(x => x.Deleted);
            Map(x => x.CreatedDate).Not.Nullable();
            
            References(x => x.Person, "PersonId");

            HasMany(x => x.Comments)
                .KeyColumns.Add("PostId", x => x.Not.Nullable())
                .Cascade.All();
        }
    }
}
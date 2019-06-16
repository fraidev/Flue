using FeedService.Domain.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps
{
    public class CommentMap: ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comment");
            Id(x => x.CommentId).GeneratedBy.Assigned();
            
            Map(x => x.Text);
            References(x => x.Person, "PersonId");
            References(x => x.Post, "PostId");
        }
    }
}
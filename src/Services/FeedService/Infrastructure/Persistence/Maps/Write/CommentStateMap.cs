using FeedService.Domain.Write.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Write
{
    public class CommentStateMap: ClassMap<CommentState>
    {
        public CommentStateMap()
        {
            Table("Comment");
            Id(x => x.Id).GeneratedBy.Assigned();;
            Map(x => x.Text);
            Map(x => x.UserId);
            References(x => x.CommentReply, "PostId");
        }
    }
}
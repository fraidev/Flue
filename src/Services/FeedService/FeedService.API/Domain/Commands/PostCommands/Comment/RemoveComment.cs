using System;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands.Comment
{
    public class RemoveComment : Command
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
    }
}
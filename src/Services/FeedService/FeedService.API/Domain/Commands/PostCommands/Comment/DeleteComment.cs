using System;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands.Comment
{
    public class DeleteComment: Command
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
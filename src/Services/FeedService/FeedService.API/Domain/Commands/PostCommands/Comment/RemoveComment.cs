using System;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands.Comment
{
    public class RemoveComment: Command
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
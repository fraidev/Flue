using System;
using FlueShared;

namespace FeedService.Domain.Commands.Post.Comment
{
    public class DeleteComment: Command
    {
        public Guid Id { get; set; }
    }
}
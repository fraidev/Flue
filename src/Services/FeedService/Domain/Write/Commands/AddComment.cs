using System;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.CQRS;
using FlueShared.CQRS;

namespace FeedService.Domain.Write.Commands
{
    public class AddComment: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public CommentState CommentReply { get; set; }
    }
}
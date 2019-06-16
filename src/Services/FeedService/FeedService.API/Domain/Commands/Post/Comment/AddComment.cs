using System;
using FlueShared;

namespace FeedService.Domain.Commands.Post.Comment
{
    public class AddComment: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public States.Person Person { get; set; }
        public States.Comment CommentReply { get; set; }
    }
}
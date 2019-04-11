using System;

namespace FeedService.Domain.Write.States
{
    public class CommentState
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public CommentState CommentReply { get; set; }
    }
}
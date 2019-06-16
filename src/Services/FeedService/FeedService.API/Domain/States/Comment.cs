using System;

namespace FeedService.Domain.States
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public Person Person { get; set; }
        public Post Post { get; set; }
        public bool IsMyComment { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace FeedService.Domain.Write.States
{
    public class PostState
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public bool OnFire { get; set; }
        public bool Deleted { get; set; }
        public IList<CommentState> Comments { get; set; } = new List<CommentState>();
    }
}

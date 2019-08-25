using System;
using System.Collections.Generic;

namespace FeedService.Domain.States
{
    public class Post
    {
        public Guid PostId { get; set; }
        public Person Person { get; set; }
        public string Text { get; set; }
        public bool Deleted { get; set; }
        public bool IsMyPost { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}

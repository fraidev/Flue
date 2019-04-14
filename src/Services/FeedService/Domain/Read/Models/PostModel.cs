using System;
using System.Collections.Generic;
using FeedService.Domain.Write.States;

namespace FeedService.Domain.Read.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public bool OnFire { get; set; }
        public bool Deleted { get; set; }
        public IList<CommentState> Comments { get; set; } = new List<CommentState>();
    }
}
using System;
using FeedService.Domain.Write.States;

namespace FeedService.Domain.Read.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid PersonId { get; set; }
        public CommentState CommentReply { get; set; }
    }
}
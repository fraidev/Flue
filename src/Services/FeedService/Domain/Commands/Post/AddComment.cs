using System;
using FeedService.Domain.States;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class AddComment: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public States.Person Person { get; set; }
        public Comment CommentReply { get; set; }
    }
}
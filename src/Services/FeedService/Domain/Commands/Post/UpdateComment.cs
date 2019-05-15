using System;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class UpdateComment: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
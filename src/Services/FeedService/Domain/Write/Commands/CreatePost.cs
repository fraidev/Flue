using System;
using FeedService.Infrastructure.CQRS;
using FlueShared;

namespace FeedService.Domain.Write.Commands
{
    public class CreatePost: Command
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}
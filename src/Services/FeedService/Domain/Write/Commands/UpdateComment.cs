using System;
using FeedService.Infrastructure.CQRS;
using FlueShared;

namespace FeedService.Domain.Write.Commands
{
    public class UpdateComment: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
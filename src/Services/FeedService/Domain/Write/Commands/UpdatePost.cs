using System;
using FeedService.Infrastructure.CQRS;

namespace FeedService.Domain.Write.Commands
{
    public class UpdatePost: Command
    {
        public string Text { get; set; }
    }
}
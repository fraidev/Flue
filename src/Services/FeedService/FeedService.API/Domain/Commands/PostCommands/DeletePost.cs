using System;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands
{
    public class DeletePost: Command
    {
        public Guid Id { get; set; }
    }
}
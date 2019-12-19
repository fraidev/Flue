using System;
using FlueShared;

namespace FeedService.Domain.Commands.PersonCommands
{
    public class UnfollowPersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public Guid UnfollowId { get; set; }
    }
}
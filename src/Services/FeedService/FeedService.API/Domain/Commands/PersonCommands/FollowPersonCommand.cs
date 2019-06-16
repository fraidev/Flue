using System;
using FlueShared;

namespace FeedService.Domain.Commands.PersonCommands
{
    public class FollowPersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public Guid FollowId { get; set; }
    }
}
using System;
using FlueShared;

namespace FeedService.Domain.Commands.Person
{
    public class FollowPersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public Guid FollowId { get; set; }
    }
}
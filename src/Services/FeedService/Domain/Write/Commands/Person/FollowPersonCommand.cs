using System;
using FlueShared;

namespace FeedService.Domain.Write.Commands.User
{
    public class FollowPersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public Guid FollowId { get; set; }
    }
}
using System;
using FlueShared;

namespace FeedService.Domain.Write.Commands.User
{
    public class FollowUserCommand: Command
    {
        public Guid UserId { get; set; }
        public Guid FollowId { get; set; }
    }
}
using System;
using FlueShared;

namespace FeedService.Domain.Write.Commands.User
{
    public class UnfollowUserCommand: Command
    {
        public Guid UserId { get; set; }
        public Guid UnfollowId { get; set; }
    }
}
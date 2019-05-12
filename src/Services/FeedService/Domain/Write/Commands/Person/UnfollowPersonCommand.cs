using System;
using FlueShared;

namespace FeedService.Domain.Write.Commands.User
{
    public class UnfollowPersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public Guid UnfollowId { get; set; }
    }
}
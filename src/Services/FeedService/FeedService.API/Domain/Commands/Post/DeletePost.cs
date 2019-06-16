using System;
using System.Collections.Generic;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class DeletePost: Command
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
    }
}
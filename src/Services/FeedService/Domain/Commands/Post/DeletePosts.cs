using System;
using System.Collections.Generic;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class DeletePosts: Command
    {
        public IList<Guid> Ids { get; set; } = new List<Guid>();
    }
}
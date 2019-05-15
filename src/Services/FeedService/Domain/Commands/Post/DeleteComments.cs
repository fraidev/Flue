using System;
using System.Collections.Generic;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class DeleteComments: Command
    {
        public IList<Guid> Ids { get; set; }
    }
}
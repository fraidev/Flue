using System;
using System.Collections.Generic;
using FeedService.Infrastructure.CQRS;

namespace FeedService.Domain.Write.Commands
{
    public class DeleteComments: Command
    {
        public IList<Guid> Ids { get; set; }
    }
}
using FeedService.Infrastructure.CQRS;
using FlueShared;

namespace FeedService.Domain.Write.Commands
{
    public class UpdatePost: Command
    {
        public string Text { get; set; }
    }
}
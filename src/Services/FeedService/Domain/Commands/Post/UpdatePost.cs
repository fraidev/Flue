using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class UpdatePost: Command
    {
        public string Text { get; set; }
    }
}
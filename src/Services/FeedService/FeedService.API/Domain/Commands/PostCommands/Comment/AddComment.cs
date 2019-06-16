using System;
using FeedService.Domain.States;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands.Comment
{
    public class AddComment: Command
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public Person Person { get; set; }
    }
}
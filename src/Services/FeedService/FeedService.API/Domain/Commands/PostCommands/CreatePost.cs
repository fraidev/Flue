using System;
using FeedService.Domain.States;
using FlueShared;

namespace FeedService.Domain.Commands.PostCommands
{
    public class CreatePost: Command
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
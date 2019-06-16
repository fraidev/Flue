using System;
using FeedService.Domain.States;
using FlueShared;

namespace FeedService.Domain.Commands.Post
{
    public class CreatePost: Command
    {
        public Guid Id { get; set; }
        public States.Person Person { get; set; }
        public string Text { get; set; }
    }
}
using System;
using FlueShared;

namespace FeedService.Domain.Commands.PersonCommands
{
    public class UpdatePersonCommand: Command
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ProfilePicture { get; set; }
    }
}
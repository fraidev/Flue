using System;

namespace FlueShared
{
    public class CreateUserCommand: Command
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}

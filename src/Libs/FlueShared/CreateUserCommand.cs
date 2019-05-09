using System;
using FlueShared.CQRS;

namespace FlueShared
{
    public class CreateUserCommand: Command
    {
        public Guid UserId { get; set; }
    }
}

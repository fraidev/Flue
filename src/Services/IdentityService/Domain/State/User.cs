using System;
using System.Collections.Generic;

namespace IdentityService.Domain.State
{
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid();
        }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
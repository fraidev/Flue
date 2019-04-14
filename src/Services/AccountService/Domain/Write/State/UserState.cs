using System;
using System.Collections.Generic;

namespace AccountService.Domain.Write.State
{
    public class UserState
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public IList<UserState> Followers { get; set; } = new List<UserState>();
        //public IList<UserState> Following { get; set; } = new List<UserState>();
    }
}
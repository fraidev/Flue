using System;
using System.Collections.Generic;

namespace FeedService.Domain.Write.States
{
    public class UserState
    {
        public Guid UserId { get; set; }
        
        public Guid IdentifyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<UserState> Following { get; set; } = new List<UserState>();
    }
}
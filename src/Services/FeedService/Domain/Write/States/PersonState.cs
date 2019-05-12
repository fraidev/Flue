using System;
using System.Collections.Generic;

namespace FeedService.Domain.Write.States
{
    public class PersonState
    {
        public Guid PersonId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<PersonState> Following { get; set; } = new List<PersonState>();
    }
}
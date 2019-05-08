using System;
using System.Collections.Generic;

namespace FeedService.Domain.Write.States
{
    public class PersonState
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<PersonState> Following { get; set; } = new List<PersonState>();
    }
}
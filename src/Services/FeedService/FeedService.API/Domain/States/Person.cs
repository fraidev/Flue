using System;
using System.Collections.Generic;

namespace FeedService.Domain.States
{
    public class Person
    {
        public Guid PersonId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<Person> Following { get; set; } = new List<Person>();
        public int FollowingCount => Following.Count;
    }
}
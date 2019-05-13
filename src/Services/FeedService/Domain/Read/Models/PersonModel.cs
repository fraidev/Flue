using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FeedService.Domain.Read.Models
{
    public class PersonModel
    {
        public Guid PersonId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public IList<PersonModel> Following { get; set; } = new List<PersonModel>();
    }
}
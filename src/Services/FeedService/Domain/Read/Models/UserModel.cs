using System;
using System.Collections.Generic;

namespace FeedService.Domain.Read.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        
        public Guid IdentifyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IList<UserModel> Following { get; set; } = new List<UserModel>();
    }
}
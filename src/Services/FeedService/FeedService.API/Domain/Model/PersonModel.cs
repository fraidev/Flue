using System;
using System.Collections.Generic;
using FeedService.Domain.States;

namespace FeedService.Domain.Model
{
    public class PersonModel
    {
        public Guid PersonId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IList<PersonModel> Following { get; set; } = new List<PersonModel>();
        public int FollowersCount => Followers.Count;
        public IList<PersonModel> Followers { get; set; } = new List<PersonModel>();
        public int FollowingCount => Following.Count;
        public IList<Post> Posts { get; set; } = new List<Post>();
        public int PostsCount => Posts.Count;
        
        
    }
}
using System;
using System.Collections.Generic;
using FeedService.Domain.States;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.Extensions;
using FlueShared;

namespace FeedService.Domain.Aggregates
{
    public class PersonAggregate:IBaseAggregate<Person>
    {
        public Guid Id { get; }
        private Person State { get; set; }
        
        
        #region Constructors
        
        public PersonAggregate(Person state)
        {
            Id = Guid.NewGuid();
            State = state;
        }
        
        public PersonAggregate(CreatePersonCommand cmd)
        {
            Id = Guid.NewGuid();
            State = new Person()
            {
                PersonId = Id,
                UserId = cmd.IdentifierId,
                Username = cmd.Username,
                Name = cmd.Name.Capitalize(),
                Description = "Eu sou novo no Flue!",
                Email = cmd.Email,
                Following = new List<Person>()
            };
        }
        
        #endregion

        public void AddPost(Post post)
        {
            GetState().Posts.Add(post);
        }
        
        public Person GetState()
        {
            return State;
        }

        public void Follow(Person person)
        {
            GetState().Following.Add(person);
        }

        public void Unfollow(Person person)
        {
            GetState().Following.Remove(person);
        }
    }
}
using System;
using System.Collections.Generic;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.CQRS;
using FlueShared;

namespace FeedService.Domain.Write.Aggregates
{
    public class PersonAggregate:IBaseAggregate<PersonState>
    {
        public Guid Id { get; }
        private PersonState State { get; set; }
        
        
        #region Constructors
        
        public PersonAggregate(PersonState state)
        {
            Id = Guid.NewGuid();
            State = state;
        }
        
        public PersonAggregate(CreatePersonCommand cmd)
        {
            Id = Guid.NewGuid();
            State = new PersonState()
            {
                PersonId = Id,
                UserId = cmd.IdentifierId,
                Username = cmd.Username,
                Name = cmd.Name,
                Email = cmd.Email,
                Following = new List<PersonState>()
            };
        }
        
        #endregion
        
        public PersonState GetState()
        {
            return State;
        }

        public void Follow(PersonState personState)
        {
            GetState().Following.Add(personState);
        }

        public void Unfollow(PersonState personState)
        {
            GetState().Following.Remove(personState);
        }
    }
}
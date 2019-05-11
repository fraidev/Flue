using System;
using System.Collections.Generic;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.CQRS;
using FlueShared;

namespace FeedService.Domain.Write.Aggregates
{
    public class UserAggregate:IBaseAggregate<UserState>
    {
        public Guid Id { get; }
        private UserState State { get; set; }
        
        
        #region Constructors
        
        public UserAggregate(UserState state)
        {
            Id = Guid.NewGuid();
            State = state;
        }
        
        public UserAggregate(CreateUserCommand cmd)
        {
            Id = Guid.NewGuid();
            State = new UserState()
            {
                UserId = Id,
                IdentifyId = cmd.IdentifierId,
                Name = cmd.Name,
                Email = cmd.Email,
                Following = new List<UserState>()
            };
        }
        
        #endregion
        
        public UserState GetState()
        {
            return State;
        }

        public void Follow(UserState userState)
        {
            GetState().Following.Add(userState);
        }

        public void Unfollow(UserState userState)
        {
            GetState().Following.Remove(userState);
        }
    }
}
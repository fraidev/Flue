using System;
using FeedService.Domain.Write.States;
using FeedService.Infrastructure.CQRS;

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
        
//        public PersonAggregate(CreatePerson cmd)
//        {
//            Id = Guid.NewGuid();
//            State = new PersonState()
//            {
//                Id = Id,
//                UserId = cmd.UserId,
//                Text = cmd.Text,
//                Comments = new List<CommentState>()
//            };
//        }
        
        #endregion
        
        public PersonState GetState()
        {
            return State;
        }
    }
}
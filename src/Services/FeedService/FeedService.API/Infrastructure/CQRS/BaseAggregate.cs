using System;

namespace FeedService.Infrastructure.CQRS
{
    public interface IBaseAggregate<TState>
    {
        Guid Id { get; }
        TState GetState();
    }
    
    public class BaseAggregate<TState>:IBaseAggregate<TState>
    {
        public Guid Id { get; }
        
        protected TState State;

        public TState GetState()
        {
            return State;
        }
    }
}
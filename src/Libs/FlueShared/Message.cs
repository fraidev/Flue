using System;
using MediatR;

namespace FlueShared
{
    public class Message : IRequest
    {
        public string MessageType { get; protected set; }
        public Type Type { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            Type = GetType();
            MessageType = Type.Name;
        }
    }
}
using System;

namespace FlueShared.CQRS
{
    public class Command: Message
    {
        public DateTime TimeStamp { get; set; }
        
        protected Command()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
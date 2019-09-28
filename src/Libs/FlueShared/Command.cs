using System;

namespace FlueShared
{
    public class Command: Message
    {
        public DateTime TimeStamp { get; set; }
        
        protected Command()
        {
            TimeStamp = DateTime.Now;
        }
    }
    public class WrapperCommand
    {
        public Command Command { get; set; }
        public Type TypeCommand { get; set; }
        
        public WrapperCommand(Command command)
        {
            Command = command;
            TypeCommand = Command.GetType();
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace CLI.CommandHandler
{
    [Serializable]
    public class CommandDispatchException : Exception
    {
        public CommandDispatchException(string message)
            : base(message)
        {
        }

        public CommandDispatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
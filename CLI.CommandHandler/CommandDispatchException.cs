using System;
using System.Runtime.Serialization;

namespace CLI.CommandHandler
{
    [Serializable]
    public class CommandDispatchException : Exception
    {
        public CommandDispatchException()
        {
        }

        public CommandDispatchException(string message)
            : base(message)
        {
        }

        public CommandDispatchException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public CommandDispatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CommandDispatchException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected CommandDispatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
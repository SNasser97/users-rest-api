namespace users_logic.Exceptions.Command
{
    using System;
    using System.Runtime.Serialization;

    public class CommandRequestException : Exception
    {
        public CommandRequestException()
        {
        }

        public CommandRequestException(string message) : base(message)
        {
        }

        public CommandRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
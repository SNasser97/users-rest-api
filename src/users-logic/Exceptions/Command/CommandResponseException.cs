namespace users_logic.Exceptions.Command
{
    using System;
    using System.Runtime.Serialization;

    public class CommandResponseException : Exception
    {
        public CommandResponseException() : base("Response Id was empty")
        {
        }

        public CommandResponseException(string message) : base(message)
        {
        }

        public CommandResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
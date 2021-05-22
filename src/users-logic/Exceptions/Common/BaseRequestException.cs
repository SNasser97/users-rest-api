namespace users_logic.Exceptions.Common
{
    using System;
    using System.Runtime.Serialization;

    public abstract class BaseRequestException : Exception
    {
        protected BaseRequestException() : base("Request Id was empty")
        {
        }

        protected BaseRequestException(string message) : base(message)
        {
        }

        protected BaseRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected BaseRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
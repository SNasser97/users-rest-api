namespace users_logic.User.Exceptions.Validation
{
    using System;
    using System.Runtime.Serialization;

    public class InvalidAgeException : Exception
    {
        public InvalidAgeException()
        {
        }

        public InvalidAgeException(string message) : base(message)
        {
        }

        public InvalidAgeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidAgeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
namespace users_logic.Exceptions.Validation
{
    using System;
    using System.Runtime.Serialization;

    public class InvalidDateOfBirthException : Exception
    {
        public InvalidDateOfBirthException()
        {
        }

        public InvalidDateOfBirthException(string message) : base(message)
        {
        }

        public InvalidDateOfBirthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDateOfBirthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
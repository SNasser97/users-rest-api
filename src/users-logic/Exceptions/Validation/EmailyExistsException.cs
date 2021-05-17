namespace users_logic.Exceptions.Validation
{
    using System;
    using System.Runtime.Serialization;

    public class EmailExistsException : Exception
    {
        public EmailExistsException() : base("Email already exists")
        {
        }

        public EmailExistsException(string message) : base(message)
        {
        }

        public EmailExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
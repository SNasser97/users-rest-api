namespace users_logic.Exceptions.Query
{
    using System;
    using System.Runtime.Serialization;

    public class QueryRequestException : Exception
    {
        public QueryRequestException(string message) : base(message)
        {
        }

        public QueryRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QueryRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
namespace users_logic.Exceptions.Query
{
    using System;
    using System.Runtime.Serialization;

    public class QueryResponseException : Exception
    {
        public QueryResponseException()
        {
        }

        public QueryResponseException(string message) : base(message)
        {
        }

        public QueryResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QueryResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
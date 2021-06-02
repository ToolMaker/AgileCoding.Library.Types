namespace AgileCoding.Library.Types.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MoreThanOneException : Exception
    {
        public MoreThanOneException()
        {
        }

        public MoreThanOneException(string message) : base(message)
        {
        }

        public MoreThanOneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

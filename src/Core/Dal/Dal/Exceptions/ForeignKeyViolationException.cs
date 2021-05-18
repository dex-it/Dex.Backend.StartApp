using System;

namespace Dal.Exceptions
{
    public class ForeignKeyViolationException : Exception
    {
        public ForeignKeyViolationException()
        {
        }

        public ForeignKeyViolationException(string message) : base(message)
        {
        }

        public ForeignKeyViolationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
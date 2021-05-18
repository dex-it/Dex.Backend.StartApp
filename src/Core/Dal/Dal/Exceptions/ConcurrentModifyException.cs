using System;

namespace Dal.Exceptions
{
    public class ConcurrentModifyException : Exception
    {
        public ConcurrentModifyException()
        {
        }

        public ConcurrentModifyException(string message) : base(message)
        {
        }

        public ConcurrentModifyException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
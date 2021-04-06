namespace Server.Dal.Exception
{
    public class ForeignKeyViolationException : System.Exception
    {
        public ForeignKeyViolationException()
        {
        }

        public ForeignKeyViolationException(string message) : base(message)
        {
        }

        public ForeignKeyViolationException(string message, System.Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
using System;
using Dal.Ef.Contract;
using Dal.Exceptions;
using Npgsql;

namespace Dal.Ef.Postgres
{
    internal class PostgresDataExceptionManager : IDataExceptionManager
    {
        public Exception Normalize(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            if (exception.InnerException is PostgresException ex)
            {
                var message = ex.Message + ex.Detail;

                switch (ex.SqlState)
                {
                    case PostgresErrorCodes.ForeignKeyViolation:
                        return new ForeignKeyViolationException(message, ex);
                    case PostgresErrorCodes.UniqueViolation:
                        return new ObjectAlreadyExistsException(message, ex);
                    case PostgresErrorCodes.SerializationFailure:
                        return new ConcurrentModifyException(message, ex);
                    default:
                        return ex;
                }
            }

            return exception;
        }

        public bool IsRepeatAction(Exception ex)
        {
            return ex is ConcurrentModifyException;
        }
    }
}
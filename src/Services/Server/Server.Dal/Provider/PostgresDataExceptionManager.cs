using System;
using System.Diagnostics.CodeAnalysis;
using Npgsql;
using Server.Dal.Contract;
using Server.Dal.Exception;

namespace Server.Dal.Provider
{
    internal class PostgresDataExceptionManager : IDataExceptionManager
    {
        public System.Exception Normalize([NotNull] System.Exception exception)
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

        public bool IsRepeatAction(System.Exception ex)
        {
            return ex is ConcurrentModifyException;
        }
    }
}
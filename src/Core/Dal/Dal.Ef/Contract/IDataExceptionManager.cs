using System;

namespace Dal.Ef.Contract
{
    internal interface IDataExceptionManager
    {
        Exception Normalize(Exception ex);
        
        bool IsRepeatAction(Exception ex);
    }
}
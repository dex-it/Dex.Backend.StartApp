namespace Server.Dal.Contract
{
    internal interface IDataExceptionManager
    {
        System.Exception Normalize(System.Exception ex);
        
        bool IsRepeatAction(System.Exception ex);
    }
}
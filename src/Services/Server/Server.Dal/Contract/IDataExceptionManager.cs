namespace Server.Dal.Contract
{
    public interface IDataExceptionManager
    {
        System.Exception Normalize(System.Exception ex);
        
        bool IsRepeatAction(System.Exception ex);
    }
}
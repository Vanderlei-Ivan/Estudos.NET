public class ApplicationNotification
{
    public int CodeStatus { get; set; }
    public string Message { get; }
    public bool IsError { get; set; }
    public DateTime TimeStamp { get; private set; }

    public ApplicationNotification(string message, int codeStatus = 400)
    {
        Message = message;
        TimeStamp = DateTime.UtcNow;
        CodeStatus = codeStatus;
        IsError = true;
    }
}
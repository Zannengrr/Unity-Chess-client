public class CallbackData
{
    public bool Success;
    public string Message;

    public CallbackData() { }
    public CallbackData(bool _success, string _message) 
    {
        Success = _success;
        Message = _message;
    }
}

public class InvitationCallback
{
    public CallbackData Data = new CallbackData(false, "");
    public string GameId;
    
    public InvitationCallback() { }
    public InvitationCallback (CallbackData _data, string _gameId)
    {
        Data = _data;
        GameId = _gameId;
    }
}

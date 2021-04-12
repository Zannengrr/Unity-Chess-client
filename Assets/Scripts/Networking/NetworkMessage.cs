public class NetworkMessage
{
    public string messageName;
    public object data;

    public NetworkMessage() { }
    public NetworkMessage(string _messageName, object _data)
    {
        messageName = _messageName;
        data = _data;
    }
}
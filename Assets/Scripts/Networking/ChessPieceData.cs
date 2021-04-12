public class ChessPieceData
{
    public string PlayerId;
    public int ChessPieceId;
    public ChessPieceType Type;
    public Side Color;

    public ChessPieceData(){}

    public ChessPieceData(string _playerId, int _id, ChessPieceType _type, Side _color)
    {
        PlayerId = _playerId;
        ChessPieceId = _id;
        Type = _type;
        Color = _color;
    }
}

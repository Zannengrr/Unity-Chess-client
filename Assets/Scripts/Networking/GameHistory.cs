public class GameHistory
{
    public string GameId;
    public BoardData Board = new BoardData();
    public PieceMoveData[] MoveHistory;
    public GameHistory() { }
    public GameHistory(string _gameId)
    {
        GameId = _gameId;
    }
    public GameHistory(string _gameId, PieceMoveData[] moveHistory)
    {
        GameId = _gameId;
        MoveHistory = moveHistory;
    }
}

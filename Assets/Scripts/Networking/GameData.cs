using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public string GameId;
    public BoardData Board;
    public string StartingPlayer;

    public GameData() { }
    public GameData(string _gameid, BoardData _board)
    {
        GameId = _gameid;
        Board = _board;
    }
}

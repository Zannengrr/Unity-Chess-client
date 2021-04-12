using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoveData
{
    public int X;
    public int Y;
    public int Id;
    public string GameId;
    public string PlayerId;

    public PieceMoveData() { }
    public PieceMoveData(int _x, int _y, int _id, string _gameId, string _playerid)
    {
        X = _x;
        Y = _y;
        Id = _id;
        GameId = _gameId;
        PlayerId = _playerid;
    }
}

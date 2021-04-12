using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionData
{
    public string UserId;
    public bool IsPublic;

    public GameSessionData() { }

    public GameSessionData(string _userId, bool _isPublic)
    {
        UserId = _userId;
        IsPublic = _isPublic;
    }
}

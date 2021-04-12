using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChessPiecesSO", menuName = "ScriptableObjects/ChessPieceSO", order = 1)]
public class ChessPiecesSO : ScriptableObject
{
    public ChessPiecesDict ChessPieces;
}

[Serializable]
public class ChessPiecesDict : SerializableDictionaryBase<ChessPieceType, GameObject> { }

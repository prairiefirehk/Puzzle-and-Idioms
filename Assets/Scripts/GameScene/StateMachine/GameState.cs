using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public enum State
    {
        IsInitalizing = 000,
        IsBattling = 100,
        PlayerWin = 200,
        PlayerLose = 300,
    }
}
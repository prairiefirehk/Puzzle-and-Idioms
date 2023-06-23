using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public enum State
    {
        IsInitalizing = 000,
        IsBattling = 100,
        IsFlying = 200,
        PlayerWin = 300,
        PlayerLose = 400,
    }
}
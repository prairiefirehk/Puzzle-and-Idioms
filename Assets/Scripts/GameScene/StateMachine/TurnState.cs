using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnState
{
    public enum State
    {
        BeforeTurnStart = 000,
        TurnStart = 001,

        // Player
        BeforePlayerMoveStart = 100,
        PlayerMoveStart = 200, 
        WaitingPlayerAction = 300,
        PlayerAction = 400,
        BeforePlayerMoveEnd = 500,
        PlayerMoveEnd = 600,

        // Mob
        BeforeMobMoveStart = 700,
        MobMoveStart = 800, 
        WaitingMobAction = 900,
        MobAction = 1000,
        BeforeMobMoveEnd = 1100,
        MobMoveEnd = 1200,

        BeforeTurnEnd = 9999,
        TurnEnd = 10000
    }
}
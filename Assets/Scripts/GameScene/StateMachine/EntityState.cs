using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityState
{
    public enum State
    {
        // For all
        IsInitalizing = 000,

        // For player and mob
        Alive = 1000,
        Dead = 1100,

        // For teammate
        Idle = 2000,
        Disable = 2100,
    }
}
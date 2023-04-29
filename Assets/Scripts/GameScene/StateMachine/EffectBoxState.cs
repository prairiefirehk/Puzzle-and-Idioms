using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBoxState
{
    public enum State
    {
        Initalizing = 000,
        Occupied = 100,
        ReadyForDestroy = 200
    }
}
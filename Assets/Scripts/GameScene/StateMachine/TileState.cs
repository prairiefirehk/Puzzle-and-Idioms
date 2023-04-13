using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileState
{
    public enum State
    {
        IsInitalizing = 000,
        
        Idle = 1000,
        Clicked = 1100,
        StartDrag = 1200,
        Dragging = 1300,
        Released = 1400,
        Processing = 1500,
        EndDrag = 1600
    }
}
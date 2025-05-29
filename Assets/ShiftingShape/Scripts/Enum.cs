using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShiftingShape
{
    public enum ShapeType : byte
    {
        None = 0,
        Human = 1,
        Bike = 2,
        Car = 3,
        Boat = 4,
        Heli = 5
    }

    public enum GameState : byte
    {
        None = 0,
        StartGame = 1,
        PauseGame = 2,
        WinGame = 3,
        LoseGame = 4,
    }

}



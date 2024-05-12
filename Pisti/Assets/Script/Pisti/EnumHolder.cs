using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumHolder : MonoBehaviour
{
    public enum PlayerColor
    {
        Red,
        Yellow,
        Blue,
        Green
    }

    public enum GameState
    {
        Running,
        Stopped
    }
    public enum PlayerState
    {
        Alive,
        Dead,
        Stunned
    }
    public enum HandState
    {
        Empty,
        Mice,
        Bomb
    }
}

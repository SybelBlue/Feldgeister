using System;
using UnityEngine;

[Serializable]
public struct RoadHookup
{
    [Serializable]
    public enum Direction
    {
        Down,
        Up,
    }

    public Direction direction;
    public Vector2Int hookupPoint;
}
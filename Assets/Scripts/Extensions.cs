﻿using System;
using UnityEngine;

public static class Extensions
{
    public static Vector3 XComponent(this Vector3 vec)
        => new Vector3(vec.x, 0, 0);

    public static Vector3 YComponent(this Vector3 vec)
        => new Vector3(0, vec.y, 0);

    public static Vector3 ZComponent(this Vector3 vec)
        => new Vector3(0, 0, vec.z);

    public static Vector2 XComponent(this Vector2 vec)
        => new Vector2(vec.x, 0);

    public static Vector2 YComponent(this Vector2 vec)
        => new Vector2(0, vec.y);

    public static Vector3 To3D(this Vector2 vec)
        => new Vector3(vec.x, vec.y, 0);

    public static bool AnyComponent(this Vector3 vec, Predicate<float> pred)
        => pred(vec.x) || pred(vec.y) || pred(vec.z);
    
    public static bool AllComponents(this Vector3 vec, Predicate<float> pred)
        => pred(vec.x) && pred(vec.y) && pred(vec.z);
}

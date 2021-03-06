﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static Vector3 XComponent(this Vector3 vec)
        => new Vector3(vec.x, 0, 0);

    public static Vector3 YComponent(this Vector3 vec)
        => new Vector3(0, vec.y, 0);

    public static Vector3 ZComponent(this Vector3 vec)
        => new Vector3(0, 0, vec.z);

    public static Vector3 ClampInCube(this Vector3 vec, Vector3 bottomCorner, Vector3 topCorner)
        => new Vector3(
            Mathf.Clamp(vec.x, bottomCorner.x, topCorner.x), 
            Mathf.Clamp(vec.y, bottomCorner.y, topCorner.y), 
            Mathf.Clamp(vec.z, bottomCorner.z, topCorner.z)
        );

    public static Vector2 XComponent(this Vector2 vec)
        => new Vector2(vec.x, 0);

    public static Vector2 YComponent(this Vector2 vec)
        => new Vector2(0, vec.y);

    public static Vector3 To3D(this Vector2 vec, float z=0)
        => new Vector3(vec.x, vec.y, z);

    public static Vector3Int ToInt(this Vector3 vec)
        => new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
    
    public static Vector2Int To2DInt(this Vector3 vec)
        => new Vector2Int((int)vec.x, (int)vec.y);

    public static Vector3Int To3D(this Vector2Int vec, int z=0)
        => new Vector3Int(vec.x, vec.y, z);

    public static Vector2Int To2D(this Vector3Int vec)
        => new Vector2Int(vec.x, vec.y);

    public static Vector3 To3DFloat(this Vector2Int vec, float z=0)
        => new Vector3(vec.x, vec.y, z);

    public static bool AnyComponent(this Vector3 vec, Predicate<float> pred)
        => pred(vec.x) || pred(vec.y) || pred(vec.z);
    
    public static bool AllComponents(this Vector3 vec, Predicate<float> pred)
        => pred(vec.x) && pred(vec.y) && pred(vec.z);

    public static Vector3 MapComponents(this Vector3 vec, Func<float, float> f)
        => new Vector3(f(vec.x), f(vec.y), f(vec.z));

    public static T RandomChoice<T>(this List<T> items)
        => items[UnityEngine.Random.Range(0, items.Count)];

    public static T MinBy<T>(this IList<T> items, Func<T, int?> selector)
        where T : class
    {
        int? lowest = null;
        T obj = null;
        foreach (T item in items)
        {
            var v = selector(item);
            if (!v.HasValue) continue;
            if (!lowest.HasValue || v.Value < lowest)
            {
                lowest = v.Value;
                obj = item;
            }
        }
        return obj;
    }

    public static T MaxBy<T>(this IList<T> items, Func<T, int?> selector)
        where T : class
    {
        int? lowest = null;
        T obj = null;
        foreach (T item in items)
        {
            var v = selector(item);
            if (!v.HasValue) continue;
            if (!lowest.HasValue || v.Value > lowest)
            {
                lowest = v.Value;
                obj = item;
            }
        }
        return obj;
    }

    // https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {
            int k = UnityEngine.Random.Range(0, n);
            n--;
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}

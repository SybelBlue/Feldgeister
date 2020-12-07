using UnityEngine;
using System.Collections.Generic;

public class RegionManager<TValue> : Dictionary<RectInt, TValue>
    where TValue : class
{
    public TValue this[Vector2Int v]
    {
        get
        { 
            foreach (KeyValuePair<RectInt, TValue> item in this)
            {
                if (item.Key.Contains(v))
                {
                    return item.Value;
                }
            }
            return null;
        }
    }

    public TValue Get(Vector2Int v)
        => this[v];

    public RectInt? GetKeyContaining(Vector2Int v)
    {
        foreach (var item in Keys)
        {
            if (item.Contains(v))
            {
                return item;
            }
        }
        return null;
    }
    

    public bool AnyContain(Vector2Int v)
    {
        foreach (var item in Keys)
        {
            if (item.Contains(v))
            {
                return true;
            }
        }
        return false;
    }

    public bool AnyOverlap(RectInt rect)
    {
        foreach (var item in Keys)
        {
            if (item.Overlaps(rect))
            {
                return true;
            }
        }
        return false;
    }
}
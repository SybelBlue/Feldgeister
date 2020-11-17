using System;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class KeystoneTile : Tile
{
    [Serializable]
    public enum PlacementStrategy
    {
        Castle,
        Graveyard,
        Shop,
        Domicile,
    }

    public PlacementStrategy strategy;
    public Vector2Int boundingBox;
    public RoadHookup[] roadHookups;
}

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

#if UNITY_EDITOR
namespace UnityEngine.Tilemaps
{
    static internal partial class AssetCreation
    {
        [MenuItem("Assets/Create/2D/Tiles/Keystone Tile",  priority = 1102)]
        static void CreateKeystoneTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<KeystoneTile>(), "New Keystone Tile.asset");
        }
    }
}
#endif
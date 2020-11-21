using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class Template : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Tilemap tilemap;


    [Serializable]
    public enum PlacementStrategy
    {
        Castle,
        Graveyard,
        Shop,
        Domicile,
    }

    public PlacementStrategy strategy;
    public RectInt boundingBox;
    public RoadHookup[] roadHookups;

    
    private void OnDrawGizmosSelected()
    {
        StaticUtils.DrawGizmoWireBox(boundingBox, Color.yellow, transform.position);
    }

    void OnValidate()
    {
        if (!tilemap)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }
}
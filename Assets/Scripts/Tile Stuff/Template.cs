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

    void OnStart()
        => tilemap = tilemap ?? GetComponent<Tilemap>();

#if UNITY_EDITOR
    public bool highlightOnMap = true;

    private void OnDrawGizmosSelected()
    {
        if (highlightOnMap)
        {
            StaticUtils.DrawGizmoWireBox(boundingBox, Color.yellow, transform.position);
        }
    }

    void OnValidate()
    {
        tilemap = tilemap ?? GetComponent<Tilemap>();
    }
#endif
}
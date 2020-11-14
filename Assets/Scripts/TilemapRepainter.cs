using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRepainter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    public GatedPerlinTile tileAsset;

    void Start()
    {
        // none of this worksssss
        // tileAsset.Rerandomize();
        // tilemap = GetComponent<Tilemap>();
        // BoundsInt bounds = tilemap.cellBounds;

        // for (int x = bounds.xMin; x < bounds.xMax; x++) {
        //     for (int y = bounds.yMin; y < bounds.yMax; y++) {
        //         var pos = new Vector3Int(x, y, 0);
        //         if (tilemap.GetTile(pos)) {
        //             tilemap.SetTile(pos, tileAsset);
        //         }
        //     }
        // }
    }
}

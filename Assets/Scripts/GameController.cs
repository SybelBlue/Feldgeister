using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UnityEngine.Tilemaps.GatedPerlinTile perlinGround;
    public UnityEngine.Tilemaps.Tilemap perlinGroundMap;
    void Awake()
    {
        // perlinGround.Rerandomize();
        // perlinGroundMap.RefreshAllTiles();
        // UnityEngine.Tilemaps.GatedPerlinTile.Rerandomize();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // will have to fix, does not respect sorting layer.
            var tile = perlinGroundMap.GetTile(new Vector3Int((int)pos.x, (int)pos.y, 0));
            if (tile) Debug.Log("Clicked " + tile);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(2, 2, 90, 20), "Refresh All"))
        {
            Debug.LogWarning("Refreshing...");
            perlinGroundMap.RefreshAllTiles();
        }
    }
}

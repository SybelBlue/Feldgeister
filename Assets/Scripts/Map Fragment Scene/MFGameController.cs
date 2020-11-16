using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFGameController : MonoBehaviour
{
    public UnityEngine.Tilemaps.Tilemap perlinGroundMap;
    
    void Awake() => RandomizeGround();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = StaticUtils.currentWorldMousePosition;
            // will have to fix, does not respect sorting layer.
            var intPos = new Vector3Int((int)pos.x, (int)pos.y, 0);
            var tile = perlinGroundMap.GetTile(intPos);
            if (tile) Debug.Log("Clicked " + intPos + " - " + tile);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(2, 2, 120, 30), "Refresh Ground"))
        {
            Debug.LogWarning("Refreshing...");
            RandomizeGround();
        }
    }

    private void RandomizeGround() => perlinGroundMap.RefreshAllTiles();
}

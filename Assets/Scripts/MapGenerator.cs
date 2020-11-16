using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField, ReadOnly] 
    private Tilemap groundTilemap, mainTilemap, darkForestTilemap;

    public Vector2Int usableMapDimensions;

    [SerializeField]
    private Tilemap castleTemplate, graveyardTemplate, houseTemplate, shop1Template, shop2Template, shopHouseTemplate;

    [SerializeField, ReadOnly]
    private List<Vector2Int> hookups;
    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<Vector2Int>();
        KeystoneTile castleKeystone = castleTemplate.GetKeystone();
        Vector2Int size = castleKeystone.boundingBox;
        hookups.AddRange(castleKeystone.roadHookups);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                mainTilemap.SetTile(pos, castleTemplate.GetTile(pos));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        groundTilemap = transform.GetChild(0)?.GetComponent<Tilemap>();
        mainTilemap = transform.GetChild(1)?.GetComponent<Tilemap>();
        darkForestTilemap = transform.GetChild(2)?.GetComponent<Tilemap>();
    }
}

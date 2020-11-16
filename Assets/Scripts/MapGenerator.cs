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
    private KeystoneTile castleKeystone, graveyardKeystone, houseKeystone, shop1Keystone, shop2Keystone, shopHouseKeystone;

    [SerializeField, ReadOnly]
    private List<Vector2Int> hookups;
    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<Vector2Int>();
        castleKeystone = LoadTemplate(castleTemplate);
        graveyardKeystone = LoadTemplate(graveyardTemplate);
    }

    KeystoneTile LoadTemplate(Tilemap template)
    {
        KeystoneTile keystone = template.GetKeystone();
        hookups.AddRange(keystone.roadHookups);
        
        Vector2Int size = keystone.boundingBox;
        Vector3Int offset = GetOffset(keystone);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                mainTilemap.SetTile(offset + pos, template.GetTile(pos));
            }
        }

        return keystone;
    }
    
    private Vector3Int GetOffset(KeystoneTile keystone)
    {
        switch (keystone.strategy)
        {
            case KeystoneTile.PlacementStrategy.Castle:
                return new Vector3Int(-keystone.boundingBox.x / 2, -keystone.boundingBox.y / 2, 0);
            case KeystoneTile.PlacementStrategy.Graveyard:
                Vector2Int placementDimensions = usableMapDimensions - keystone.boundingBox;
                int perimeter = 2 * (placementDimensions.x + placementDimensions.y);
                int loc = Random.Range(0, perimeter);
                
                if (loc < placementDimensions.y) return new Vector3Int(0, loc, 0);
                loc -= placementDimensions.y;
                
                if (loc < placementDimensions.x) return new Vector3Int(loc, placementDimensions.y, 0);
                loc -= placementDimensions.x;
                
                if (loc < placementDimensions.y) return new Vector3Int(placementDimensions.x, loc, 0);
                loc -= placementDimensions.y;
                
                return new Vector3Int(loc, 0, 0);
        }
        return Vector3Int.zero;
    } 

    void OnValidate()
    {
        groundTilemap = transform.GetChild(0)?.GetComponent<Tilemap>();
        mainTilemap = transform.GetChild(1)?.GetComponent<Tilemap>();
        darkForestTilemap = transform.GetChild(2)?.GetComponent<Tilemap>();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField, ReadOnly] 
    private Tilemap groundTilemap, mainTilemap, darkForestTilemap;

    public Vector2Int usableMapDimensions;

    private Vector2Int bottomLeftCorner 
        => new Vector2Int(-usableMapDimensions.x / 2, -usableMapDimensions.y / 2);
    private Vector2Int upperRightCorner 
        => bottomLeftCorner + usableMapDimensions;
    private RectInt mapBounds 
        => new RectInt(bottomLeftCorner.x, bottomLeftCorner.y, usableMapDimensions.x, usableMapDimensions.y);

    [SerializeField]
    private Tilemap castleTemplate, graveyardTemplate, houseTemplate, shop1Template, shop2Template, shopHouseTemplate;

    [SerializeField, ReadOnly] 
    private KeystoneTile castleKeystone, graveyardKeystone, house1Keystone, house2Keystone, house3Keystone, shop1Keystone, shop2Keystone, shopHouseKeystone;

    [SerializeField, ReadOnly]
    private List<Vector2Int> hookups;

    [SerializeField, ReadOnly]
    private List<RectInt> usedSpaces;

    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<Vector2Int>();
        usedSpaces = new List<RectInt>();

        // do castle first, always centered.
        castleKeystone = LoadTemplate(castleTemplate);
        // do graveyard next, always on an edge.
        graveyardKeystone = LoadTemplate(graveyardTemplate);

        // add 2 houses and 2 shops
        house1Keystone = LoadTemplate(houseTemplate);
        house2Keystone = LoadTemplate(houseTemplate);

        shop1Keystone = LoadTemplate(shop1Template);
        shop2Keystone = LoadTemplate(shop2Template);
    }

    KeystoneTile LoadTemplate(Tilemap template)
    {
        KeystoneTile keystone = template.GetKeystone();
        hookups.AddRange(keystone.roadHookups);
        
        Vector2Int size = keystone.boundingBox;
        Vector3Int offset = GetOffset(keystone);
        
        RectInt usedSpace = new RectInt(offset.x, offset.y, size.x, size.y);
        usedSpaces.Add(usedSpace);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                Vector3Int mainPos = offset + pos;
                mainTilemap.SetTile(mainPos, template.GetTile(pos));
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
                Vector3Int rawPos = RawRandomGraveyardPosition(placementDimensions);
                return rawPos + bottomLeftCorner.To3D();
            default:
                var dimensions = keystone.boundingBox;
                Vector3Int pos;
                RectInt usedSpace;
                do
                {
                    pos = new Vector3Int(
                        Random.Range(bottomLeftCorner.x, upperRightCorner.x - dimensions.x),
                        Random.Range(bottomLeftCorner.y, upperRightCorner.y - dimensions.y),
                        0
                    );
                    usedSpace = new RectInt(pos.x, pos.y, dimensions.x, dimensions.y);
                } while (usedSpaces.Any(rect => rect.Overlaps(usedSpace)));

                return pos;
        }
        return Vector3Int.zero;
    }

    private static Vector3Int RawRandomGraveyardPosition(Vector2Int placementDimensions)
    {
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

    void OnValidate()
    {
        groundTilemap = transform.GetChild(0)?.GetComponent<Tilemap>();
        mainTilemap = transform.GetChild(1)?.GetComponent<Tilemap>();
        darkForestTilemap = transform.GetChild(2)?.GetComponent<Tilemap>();
    }
}

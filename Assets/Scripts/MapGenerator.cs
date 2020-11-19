using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using AStarSharp;

#pragma warning disable 0649
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

    [SerializeField]
    private TileBase roadTile, riverTile;

    [SerializeField]
    private GatedPerlinTile groundTile;

    [SerializeField, ReadOnly] 
    private KeystoneTile castleKeystone, graveyardKeystone, house1Keystone, house2Keystone, house3Keystone, house4Keystone, shop1Keystone, shop2Keystone, shopHouseKeystone;

    [SerializeField, ReadOnly]
    private List<RoadHookup> hookups;

    [ReadOnly]
    public readonly RegionManager<string> usedSpaces = new RegionManager<string>();

    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<RoadHookup>();

        // do castle first, always centered.
        castleKeystone = LoadTemplate(castleTemplate, "castle");
        // do graveyard next, always on an edge.
        graveyardKeystone = LoadTemplate(graveyardTemplate, "graveyard");

        // add 2 houses
        house1Keystone = LoadTemplate(houseTemplate, "house1");
        house2Keystone = LoadTemplate(houseTemplate, "house2");

        // add 2 shops
        shop1Keystone = LoadTemplate(shop1Template, "shop1");
        shop2Keystone = LoadTemplate(shop2Template, "shop2");

        // add shop&house
        shopHouseKeystone = LoadTemplate(shopHouseTemplate, "shopHouse");

        // add 2 more houses (now 5, enough for each character)
        house3Keystone = LoadTemplate(houseTemplate, "house3");
        house4Keystone = LoadTemplate(houseTemplate, "house4");

        // make roads
        // make river
        // LoadRiver();
        // make fields
        // make wells, rocks?
    }

    private void LoadRiver()
    {
        var grid = new List<List<Node>>();
        for (int i = bottomLeftCorner.x - 5; i < upperRightCorner.x + 5; i++)
        {
            var row = new List<Node>();
            for (int j = bottomLeftCorner.y - 5; j < upperRightCorner.y + 5; j++)
            {
                row.Add(MakeMapNode(new Vector2Int(i, j)));
            }
            grid.Add(row);
        }
        Astar astar = new Astar(grid);

        var start = -castleKeystone.boundingBox / 2 - bottomLeftCorner + new Vector2Int(5, 5);
        var end = new Vector2Int(1, 1);

        var pathStack = astar.FindPath(start, end);

        if (pathStack != null) 
        {
            foreach (var item in pathStack)
            {
                var pos = item.Position + bottomLeftCorner - new Vector2Int(5, 5);
                mainTilemap.SetTile(pos.To3D(), riverTile);
            }
        }

        start = castleKeystone.boundingBox / 2 - bottomLeftCorner + new Vector2Int(4, 4);
        end = usableMapDimensions + new Vector2Int(8, 8);

        pathStack = astar.FindPath(start, end);
        if (pathStack != null) 
        {
            foreach (var item in pathStack)
            {
                var pos = item.Position + bottomLeftCorner - new Vector2Int(5, 5);
                mainTilemap.SetTile(pos.To3D(), riverTile);
            }
        }
    }

    private Node MakeMapNode(Vector2Int vec)
        => new Node(vec - bottomLeftCorner + new Vector2Int(5, 5), !usedSpaces.AnyContain(vec), 10 * groundTile.RawPerlinValue(vec.x, vec.y));

    private KeystoneTile LoadTemplate(Tilemap template, string name)
    {
        KeystoneTile keystone = template.GetKeystone();
        hookups.AddRange(keystone.roadHookups);
        
        Vector2Int size = keystone.boundingBox;
        Vector3Int offset = GetOffset(keystone);
        
        RectInt usedSpace = new RectInt(offset.x, offset.y, size.x, size.y);
        usedSpaces.Add(usedSpace, name);

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
                Vector3Int rawPos = RandomPositionOnPerimeter(placementDimensions);
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
                } while (usedSpaces.AnyOverlap(usedSpace));

                return pos;
        }
    }

    private static Vector3Int RandomPositionOnPerimeter(Vector2Int placementDimensions)
    {
        // one coordinate is always either 0 or maxed out, the other coordinate is a random value in [0, bound]

        // pick a 0 or maxed coordinate
        bool staticX = StaticUtils.randomBool;
        // get its max value
        int staticBound = staticX ? placementDimensions.x : placementDimensions.y;
        // pick 0 or max value
        int staticValue = StaticUtils.randomBool ? staticBound : 0;
        
        // get the max rand value
        int randBound = !staticX ? placementDimensions.x : placementDimensions.y;
        // pick a random value in the range
        int randValue = Random.Range(0, randBound);

        // return
        return new Vector3Int(
            staticX ? staticValue : randValue, 
            staticX ? randValue : staticValue, 
            0
        );
    }

    void OnValidate()
    {
        groundTilemap = transform.GetChild(0)?.GetComponent<Tilemap>();
        mainTilemap = transform.GetChild(1)?.GetComponent<Tilemap>();
        darkForestTilemap = transform.GetChild(2)?.GetComponent<Tilemap>();
    }
}

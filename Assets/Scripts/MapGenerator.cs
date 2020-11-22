using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using AStarSharp;
using static Character;

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

    private KeystoneTile castleKeystone;
    private KeystoneTile graveyardKeystone;
    private KeystoneTile house1Keystone, house2Keystone, house3Keystone, house4Keystone;
    private KeystoneTile shop1Keystone, shop2Keystone;
    private KeystoneTile shopHouseKeystone;

    [SerializeField, ReadOnly]
    private List<RoadHookup> hookups;

    [ReadOnly]
    public readonly RegionManager<Building> usedSpaces = new RegionManager<Building>();

    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<RoadHookup>();

        // do castle first, always centered.
        castleKeystone = LoadBuilding(castleTemplate, "castle");
        // do graveyard next, always on an edge.
        graveyardKeystone = LoadBuilding(graveyardTemplate, "graveyard");

        // add 2 houses
        house1Keystone = LoadHouse(houseTemplate, "house1", Miner);
        house2Keystone = LoadHouse(houseTemplate, "house2", Blacksmith);

        // add 2 shops
        shop1Keystone = LoadBuilding(shop1Template, "shop1");
        shop2Keystone = LoadBuilding(shop2Template, "shop2");

        // add shop&house
        shopHouseKeystone = LoadBuilding(shopHouseTemplate, "shopHouse");

        // add 2 more houses (now 5, enough for each character)
        house3Keystone = LoadHouse(houseTemplate, "house3", Watcher);
        house4Keystone = LoadHouse(houseTemplate, "house4", Witch);

        // make roads
        // make river
        // LoadRiver();
        // make fields
        // make wells, rocks?
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            var flattened = StaticUtils.currentWorldMousePosition.To2DInt();
            print(usedSpaces[flattened]?.buildingName);
            print(mainTilemap.GetTile(flattened.To3D()));
        }
    }

    private KeystoneTile LoadHouse(Tilemap template, string name, Character character)
    {
        KeystoneTile keystone = LoadTemplate(template, out RectInt usedSpace);
        House house = House.AddTo(gameObject, name, character, usedSpace);
        usedSpaces.Add(usedSpace, house);
        return keystone;
    }

    private KeystoneTile LoadBuilding(Tilemap template, string name)
    {
        KeystoneTile keystone = LoadTemplate(template, out RectInt usedSpace);
        Building building = Building.AddTo(gameObject, name, usedSpace);
        usedSpaces.Add(usedSpace, building);
        return keystone;
    }

    private KeystoneTile LoadTemplate(Tilemap template)
        => LoadTemplate(template, out RectInt _);
    
    private KeystoneTile LoadTemplate(Tilemap tilemap, out RectInt usedSpace)
    {
        Template template = tilemap.GetComponent<Template>();
        hookups.AddRange(template.roadHookups);
        
        Vector2Int size = template.boundingBox.size;
        Vector3Int offset = GetOffset(template);
        
        usedSpace = new RectInt(offset.x, offset.y, size.x, size.y);

        foreach (Vector3Int pos in template.boundingBox.allPositionsWithin)
        {
            Vector3Int mainPos = offset + pos - template.boundingBox.position.To3D();
            mainTilemap.SetTile(mainPos, tilemap.GetTile(pos));
        }

        return null;
    }
    
    private Vector3Int GetOffset(Template template)
    {
        var size = template.boundingBox.size;
        switch (template.strategy)
        {
            case Template.PlacementStrategy.Castle:
                return new Vector3Int(-size.x / 2, -size.y / 2, 0);
            case Template.PlacementStrategy.Graveyard:
                Vector2Int placementDimensions = usableMapDimensions - size;
                Vector3Int rawPos = RandomPositionOnPerimeter(placementDimensions);
                return rawPos + bottomLeftCorner.To3D();
            default:
                Vector3Int pos;
                RectInt usedSpace;
                do
                {
                    pos = new Vector3Int(
                        Random.Range(bottomLeftCorner.x, upperRightCorner.x - size.x),
                        Random.Range(bottomLeftCorner.y, upperRightCorner.y - size.y),
                        0
                    );
                    usedSpace = new RectInt(pos.x, pos.y, size.x, size.y);
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

#if UNITY_EDITOR
    public bool showMapBounds = true;

    private void OnDrawGizmosSelected()
    {
        if (showMapBounds)
        {
            StaticUtils.DrawGizmoWireBox(mapBounds, Color.blue, transform.position);
        }
    }
#endif

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
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using System.Linq;

using static CharacterClass;

[System.Serializable]
public class MapEvent : UnityEvent<MapGenerator>
{  }

#pragma warning disable 0649
public class MapGenerator : MonoBehaviour
{
    public MapEvent onMapMade;

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
    private Tilemap castleTilemap, graveyardTilemap, houseTilemap, shop1Tilemap, shop2Tilemap, shopHouseTilemap;

    [SerializeField]
    private TileBase roadTile, riverTile;

    [SerializeField]
    private GatedPerlinTile groundTile;

    public BuildingDefense[] giveEachHouse;

    private Template castleTemplate;
    private Template graveyardTemplate;
    private Template house1Template, house2Template, house3Template, house4Template;
    private Template shop1Template, shop2Template, shopHouseTemplate;

    [SerializeField, ReadOnly]
    private List<RoadHookup> hookups;

    public readonly RegionManager<Building> usedSpaces = new RegionManager<Building>();

    // Start is called before the first frame update
    void Start()
    {
        hookups = new List<RoadHookup>();

        // do castle first, always centered.
        castleTemplate = LoadBuilding(castleTilemap, "castle");
        // do graveyard next, always on an edge.
        graveyardTemplate = LoadBuilding(graveyardTilemap, "graveyard");

        // add 2 houses
        house1Template = LoadHouse(houseTilemap, "house1", Miner);
        house2Template = LoadHouse(houseTilemap, "house2", Blacksmith);

        // add 2 shops
        shop1Template = LoadBuilding(shop1Tilemap, "shop1");
        shop2Template = LoadBuilding(shop2Tilemap, "shop2");

        // add shop&house
        shopHouseTemplate = LoadBuilding(shopHouseTilemap, "shopHouse");

        // add 2 more houses (now 5, enough for each character)
        house3Template = LoadHouse(houseTilemap, "house3", Watcher);
        house4Template = LoadHouse(houseTilemap, "house4", Witch);

        // make roads
        // make river
        // LoadRiver();
        // make fields
        // make wells, rocks?

        onMapMade.Invoke(this);
    }

    private Template LoadHouse(Tilemap tilemap, string name, CharacterClass character)
    {
        Template template = LoadTemplate(tilemap, out RectInt usedSpace);
        House house = House.AddTo(gameObject, name, character, usedSpace);
        house.defenses = giveEachHouse.Select(defense => Instantiate(defense)).ToList();
        usedSpaces.Add(usedSpace, house);
        return template;
    }

    private Template LoadBuilding(Tilemap tilemap, string name)
    {
        Template template = LoadTemplate(tilemap, out RectInt usedSpace);
        Building building = Building.AddTo(gameObject, name, usedSpace);
        usedSpaces.Add(usedSpace, building);
        return template;
    }

    private Template LoadTemplate(Tilemap tilemap)
        => LoadTemplate(tilemap, out RectInt _);
    
    private Template LoadTemplate(Tilemap tilemap, out RectInt usedSpace)
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

        return template;
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
}

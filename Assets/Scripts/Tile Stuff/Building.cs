using UnityEngine;

public class Building : MonoBehaviour, IRegion
{
    public string buildingName;
    public RectInt region { get; set; }

    public static Building AddTo(GameObject gameObject, string name, RectInt region)
    {
        Building building = gameObject.AddComponent<Building>() as Building;
        building.buildingName = name;
        building.region = region;
        return building;
    }
    private void OnDrawGizmosSelected()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        
        Vector3 position = region.position.To3DFloat() + region.size.To3DFloat() / 2.0f;
        Gizmos.DrawWireCube(position, region.size.To3DFloat() + Vector3.up);

        Gizmos.color = oldColor;
    }
}
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
        StaticUtils.DrawGizmoWireBox(region, Color.yellow, transform.position);
    }
}
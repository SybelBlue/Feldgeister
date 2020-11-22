using UnityEngine;

public class Building : MonoBehaviour, IRegion
{
    [ReadOnly]
    public string buildingName;
    public RectInt region { get; set; }

    public static Building AddTo(GameObject gameObject, string name, RectInt region)
    {
        Building building = gameObject.AddComponent<Building>() as Building;
        building.buildingName = name;
        building.region = region;
        return building;
    }

#if UNITY_EDITOR
    public bool highlightOnMap = true;

    private void OnDrawGizmosSelected()
    {
        if (highlightOnMap)
        {
            StaticUtils.DrawGizmoWireBox(region, Color.yellow, transform.position);
        }
    }
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private RegionManager<Building> buildingMap;

    public bool mapReady { get => mapGenerator != null; }

#if UNITY_EDITOR
    // used only to display progress in inspector!
    [SerializeField, ReadOnly]
    private bool _mapReady = false;
#endif

    public void OnMapMade(MapGenerator map)
    {
        mapGenerator = map;
        buildingMap = map.usedSpaces;
        _mapReady = true;
    }

    void Update()
    {
        if (mapReady && Input.GetMouseButtonDown(0)) 
        {
            var flattened = StaticUtils.currentWorldMousePosition.To2DInt();
            print(buildingMap[flattened]?.buildingName);
        }
    }
}

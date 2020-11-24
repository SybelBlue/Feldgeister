﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private RegionManager<Building> buildingMap;

    public bool mapReady { get => mapGenerator != null; }

#if UNITY_EDITOR
    // used only to display progress in inspector!
#pragma warning disable 0414
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
        if (Feldgeister.Input.mouseHasMoved && mapReady) 
        {
            var flattened = Feldgeister.Input.currentWorldMousePosition.To2DInt();
            Feldgeister.Input.lastFocused = buildingMap[flattened];
        }

        if (Input.GetMouseButtonDown(0))
        {
            Feldgeister.Input.SendClick();
        }
    }
}
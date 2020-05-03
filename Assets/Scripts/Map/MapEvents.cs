﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEvents : MonoBehaviour
{
    public static MapEvents GetMapEvents { get; private set; }

    public static event Action<Tilemap> WorldMapChange;

    private PlayerMain gottenPlayer;

    private PlayerMain Player
    {
        get
        {
            if (gottenPlayer == null) { gottenPlayer = PlayerMain.GetPlayer; }
            return gottenPlayer;
        }
    }

    public static Tilemap CurrentMap { get; private set; }

    [SerializeField] private Tilemap startMap = null;

    public static WorldMaps ActiveMap { get; private set; }
    private WorldMap CurrentWorld => worldMaps.Find(m => m.World == ActiveMap);

    [SerializeField] private List<WorldMap> worldMaps = new List<WorldMap>();

    private List<Transform> WorldChildren
    {
        get
        {
            return new List<Transform>(CurrentWorld.GetComponentsInChildren<Transform>());
        }
    }

    #region mapScript

    private List<Map> lastMaps = new List<Map>();

    private List<Map> GetMaps
    {
        get
        {
            if (lastMaps.Count < 1)
            {
                lastMaps = CurrentWorld.Maps;
            }
            return lastMaps;
        }
    }

    private Map lastMap;
    private static bool mapDirty = true;

    public Map CurMapScript
    {
        get
        {
            if (mapDirty)
            {
                lastMap = GetMaps.Find(m => m.name == CurrentMap.name);
                mapDirty = false;
            }
            return lastMap;
        }
    }

    #endregion mapScript

    private void Awake()
    {
        if (GetMapEvents == null)
        {
            GetMapEvents = this;
        }
        else if (GetMapEvents != this)
        {
            Destroy(gameObject);
        }
        if (startMap != null)
        {
            CurrentMap = startMap;
        }
        else
        {
            Debug.LogError("MapEvents had to pick random startmap, not good...");
            CurrentMap = GetComponent<Tilemap>();
        }
        worldMaps = new List<WorldMap>(GetComponentsInChildren<WorldMap>());
        transform.SleepChildren(CurrentWorld.transform);
        mapDirty = true;
    }

    private List<TelePortLocation> telePortLocations;

    public List<TelePortLocation> TelePortLocations
    {
        get
        {
            if (telePortLocations == null)
            {
                telePortLocations = new List<TelePortLocation>();
                foreach (CanTelePortTo canTele in GetComponentsInChildren<CanTelePortTo>())
                {
                    telePortLocations.Add(new TelePortLocation(canTele, this));
                }
            }
            return telePortLocations;
        }
    }

    private void Start()
    {
    }

    public static void MapChange(Tilemap newMap)
    {
        mapDirty = true;
        CurrentMap = newMap;
        WorldMapChange?.Invoke(CurrentMap);
    }

    public void WorldChange(WorldMaps newWorld, Tilemap newMap)
    {
        transform.SleepChildren();
        ActiveMap = newWorld;
        CurrentWorld.gameObject.SetActive(true);
        MapChange(newMap);
    }

    public void Teleport(WorldMaps toWorld, Tilemap toMap)
    {
        Player.transform.position = toMap.cellBounds.center;
        WorldChange(toWorld, toMap);
    }

    public void Teleport(WorldMaps toWorld, Tilemap toMap, Vector3 landPos)
    {
        Player.transform.position = landPos;
        WorldChange(toWorld, toMap);
    }

    public void Teleport(WorldMaps toWorld, Tilemap toMap, Tilemap teleportPlatform)
    {
        Player.transform.position = teleportPlatform == null ? toMap.cellBounds.center : teleportPlatform.cellBounds.center;
        WorldChange(toWorld, toMap);
    }

    public void Load(PosSave save)
    {
        ActiveMap = save.World;
        WorldChange(save.World, WorldChildren.Find(m => m.name == save.Map).transform.gameObject.GetComponent<Tilemap>());
        Player.transform.position = save.Pos;
    }
}
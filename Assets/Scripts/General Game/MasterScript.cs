﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour
{

    static int currentFloor;
    static Dungeon currentDungeon;
    static GameObject wasteBin;
    public static Dictionary<string, GameObject> roomMeshes = new Dictionary<string, GameObject>();
    public static float tileWidth, tileHeight;
    public static bool stopMovement;
    public static bool stopControls;
    static bool inEncounter;

    // Use this for initialization
    void Awake()
    {
        currentFloor = 0;
        stopMovement = false;
        wasteBin = GameObject.Find("_Wastebin");
        GameObject groundTile = Resources.Load<GameObject>("Background Prefabs/Base Tile");
        tileWidth = groundTile.renderer.bounds.size.x;
        tileHeight = groundTile.renderer.bounds.size.y;
    }

    public static void NextFloor()
    {
        currentFloor += 1;
        Cleanup();
        currentDungeon = new Dungeon(currentFloor);
    }

    public static Vector3 GetRealPosition(Vector2 localPosition)
    {
        return new Vector3(localPosition.x * tileWidth, localPosition.y * tileHeight, 0);
    }

    public static int CurrentFloor
    {
        get { return currentFloor; }
    }

    public static Dungeon CurrentDungeon
    {
        get { return currentDungeon; }
    }

    public static bool InEncounter
    {
        get { return inEncounter; }
        set
        {
            inEncounter = value;
            if (inEncounter)
            {
                stopMovement = true;
            }
            else
            {
                stopMovement = false;
            }
        }
    }

    static void Cleanup()
    {
        if (wasteBin == null)
            return;
        foreach (Transform g in wasteBin.GetComponentInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }
        roomMeshes.Clear();
    }

}

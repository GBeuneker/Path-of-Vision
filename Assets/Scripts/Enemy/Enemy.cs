using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enemy
{
    string path;
    Vector2 localPosition;
    Room room;

    public Enemy(string path, Room room)
    {
        this.path = path;
        this.room = room;
    }

    public Vector2 LocalPosition
    {
        get { return localPosition; }
        set { localPosition = value; }
    }

    public string Path
    {
        get { return this.path; }
    }

    public Room Room
    {
        get { return this.room; }
    }

    public GameObject enemyObject
    {
        get { return Resources.Load<GameObject>(this.path); }
    }

    public float TriggerRange
    {
        get;
        set;
    }
}


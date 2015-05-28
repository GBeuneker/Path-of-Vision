using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestRoom : Room
{
    GameObject chest; 

    public ChestRoom(List<string> roomLayout)
        : base(roomLayout)
    {
        this.roomLayout = roomLayout;
        chest = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Room Prefabs/Chest"));
        chest.transform.SetParent(GameObject.Find("_Wastebin").transform);
        chest.SetActive(false);
    }

    public void DestroyChest()
    {
        MonoBehaviour.Destroy(this.chest);
    }

    public GameObject Chest
    {
        get { return this.chest; }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList : MonoBehaviour
{
    public List<GameObject> allItems;

    public GameObject RandomItem()
    {
        int randomNumber = RandomGenerator.RandomInt(0, allItems.Count);
        return allItems[randomNumber];
    }
}

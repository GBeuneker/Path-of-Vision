using UnityEngine;
using System.Collections;

public class ContainerBehaviour : MonoBehaviour
{

    int amountOfSlots;
    ItemList itemList;

    // Use this for initialization
    void Start()
    {
        amountOfSlots = transform.childCount;
        itemList = GameObject.Find("_Game Master").GetComponent<ItemList>();
        FillSlots(1);
    }

    public void FillSlots(int amount, float rarity = 0)
    {
        for (int i = 0; i < amount; ++i)
        {
            if (i >= amountOfSlots)
                return;
            GameObject item = (GameObject)Instantiate(itemList.RandomItem());
            item.transform.SetParent(transform.GetChild(i));
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector2.zero;
            item.GetComponent<Dragging>().originalParent = transform.GetChild(i);
            item.GetComponent<Dragging>().originalPosition = item.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;
using System.Collections;
using System;

public class ItemSlotScript : MonoBehaviour
{
    public GameObject player;
    public GameObject backpack;
    bool isOpen;

    public enum ItemSlot
    {
        LeftHand, RightHand, Head, Body, Feet
    }

    public ItemSlot itemSlot;

    // Use this for initialization
    void Start()
    {
    }

    public void EquipItem(GameObject item)
    {
        if (!Compatible(item))
            return;

        if (this.Item != null)
            SwapItems(this.Item, item);

        item.transform.SetParent(transform);
        item.transform.localScale = Vector3.one;
        item.transform.position = transform.position;
        if (item.GetComponent<Dragging>() != null)
        {
            item.GetComponent<Dragging>().originalPosition = transform.position;
            item.GetComponent<Dragging>().originalParent = transform;
        }
        player.GetComponent<Equipment>().UpdateItems();

        return;
    }

    public void SwapItems(GameObject item1, GameObject item2)
    {
        item1.GetComponent<Dragging>().originalPosition = item2.GetComponent<Dragging>().originalPosition;
        item1.transform.position = item2.GetComponent<Dragging>().originalPosition;
        item1.GetComponent<Dragging>().originalParent = item2.GetComponent<Dragging>().originalParent;
        item1.transform.SetParent(item2.GetComponent<Dragging>().originalParent);
    }

    public void Unequip()
    {
        player.GetComponent<Equipment>().UpdateItems();
    }

    public bool Compatible(GameObject item)
    {
        WeaponStats itemStats = item.GetComponent<WeaponStats>();
        EquipScript equipScript = GameObject.Find("Equip").GetComponent<EquipScript>();

        if (itemStats.itemType == WeaponStats.ItemType.OneHanded)
        {
            if (equipScript.rightHand.GetComponent<ItemSlotScript>().Item != null)
            {
                if (equipScript.rightHand.GetComponent<ItemSlotScript>().Item.GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
                    return false;
            }
            if (equipScript.leftHand.GetComponent<ItemSlotScript>().Item != null)
            {
                if (equipScript.leftHand.GetComponent<ItemSlotScript>().Item.GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
                    return false;
            }
            if (itemSlot == ItemSlot.LeftHand || itemSlot == ItemSlot.RightHand)
                return true;

        }
        else if (itemStats.itemType == WeaponStats.ItemType.TwoHanded)
        {
            if (itemSlot == ItemSlot.LeftHand)
            {
                if (equipScript.rightHand.GetComponent<ItemSlotScript>().Item == null)
                    return true;
            }
            else if (itemSlot == ItemSlot.RightHand)
            {
                if (equipScript.leftHand.GetComponent<ItemSlotScript>().Item == null)
                    return true;
            }
        }
        else if (itemStats.itemType == WeaponStats.ItemType.Head)
        {
            if (itemSlot == ItemSlot.Head)
                return true;
        }
        else if (itemStats.itemType == WeaponStats.ItemType.Body)
        {
            if (itemSlot == ItemSlot.Body)
                return true;
        }
        else if (itemStats.itemType == WeaponStats.ItemType.Feet)
        {
            if (itemSlot == ItemSlot.Body)
                return true;
        }

        return false;
    }

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            else
                return null;
        }
    }
}

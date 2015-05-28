using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Equipment : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public GameObject head, body, feet;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateItems()
    {
        List<GameObject> itemList = new List<GameObject>();
        itemList.Add(leftHand.GetComponent<ItemSlotScript>().Item);
        itemList.Add(rightHand.GetComponent<ItemSlotScript>().Item);
        itemList.Add(head.GetComponent<ItemSlotScript>().Item);
        itemList.Add(body.GetComponent<ItemSlotScript>().Item);
        itemList.Add(feet.GetComponent<ItemSlotScript>().Item);

        GetComponent<Stats>().CurrentDefence = GetComponent<Stats>().defence;
        GetComponent<Stats>().CurrentIntelligence = GetComponent<Stats>().intelligence;
        GetComponent<Stats>().CurrentMaxHealth = GetComponent<Stats>().maxHealth;
        GetComponent<Stats>().CurrentDexterity = GetComponent<Stats>().dexterity;

        foreach (GameObject item in itemList)
        {
            if (item == null)
                continue;
            WeaponStats stats = item.GetComponent<WeaponStats>();
            GetComponent<Stats>().CurrentDefence += stats.defence;
            GetComponent<Stats>().CurrentIntelligence += stats.intelligence;
            GetComponent<Stats>().CurrentMaxHealth += stats.maxHealth;
            GetComponent<Stats>().CurrentDexterity += stats.dexterity;
        }

    }

    public GameObject LeftHand
    {
        get
        {
            if (leftHand.GetComponent<ItemSlotScript>().Item == null)
                return Resources.Load<GameObject>("Item Prefabs/Weapons/LeftHand");
            else
                return leftHand.GetComponent<ItemSlotScript>().Item;
        }
    }

    public GameObject RightHand
    {
        get
        {
            if (rightHand.GetComponent<ItemSlotScript>().Item == null)
                return Resources.Load<GameObject>("Item Prefabs/Weapons/RightHand");
            else
                return rightHand.GetComponent<ItemSlotScript>().Item;
        }
    }

    public GameObject Head
    {
        get
        {
            if (head.GetComponent<ItemSlotScript>().Item == null)
                return Resources.Load<GameObject>("Item Prefabs/Armour/Head");
            else
                return head.GetComponent<ItemSlotScript>().Item;
        }
    }

    public GameObject Body
    {
        get
        {
            if (body.GetComponent<ItemSlotScript>().Item == null)
                return Resources.Load<GameObject>("Item Prefabs/Armour/Body");
            else
                return body.GetComponent<ItemSlotScript>().Item;
        }
    }

    public GameObject Feet
    {
        get
        {
            if (feet.GetComponent<ItemSlotScript>().Item == null)
                return Resources.Load<GameObject>("Item Prefabs/Armour/Feet");
            else
                return feet.GetComponent<ItemSlotScript>().Item;
        }
    }
}

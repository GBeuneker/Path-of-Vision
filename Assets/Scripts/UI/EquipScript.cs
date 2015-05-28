using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EquipScript : MonoBehaviour
{
    public GameObject equipmentScreen;
    public GameObject leftHand, rightHand, head, body, feet;
    public bool enabled;

    public GameObject leftEquip, rightEquip;

    GameObject backpack, player;
    // Use this for initialization
    void Start()
    {
        enabled = true;
        backpack = GameObject.Find("Backpack");
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Image>().color.a < 1)
            return;
        if (Input.GetMouseButtonDown(0) && transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
        {
            if (!equipmentScreen.activeSelf)
            {
                GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f);
                enabled = false;
                equipmentScreen.SetActive(true);
                player.GetComponent<Equipment>().UpdateItems();
                MasterScript.stopMovement = true;
                if (backpack.GetComponent<Backpack>().ItemContainer != null)
                    backpack.GetComponent<Backpack>().ItemContainer.SetActive(false);
            }
            else
            {
                GetComponent<Image>().color = Color.white;
                enabled = true;
                equipmentScreen.SetActive(false);
                MasterScript.stopMovement = false;
                if (backpack.GetComponent<Backpack>().ItemContainer != null)
                    backpack.GetComponent<Backpack>().ItemContainer.SetActive(true);
            }
        }
    }

    public GameObject CorrectItemSlot(GameObject item, string hand = "")
    {
        if(hand != "")
        {
            if (hand == "Left")
                return leftHand;
            else if (hand == "Right")
                return rightHand;
            else
                throw new Exception("Wrong input for hand equipping! (Error 007)");
        }

        switch (item.GetComponent<WeaponStats>().itemType)
        {
            case WeaponStats.ItemType.TwoHanded:
                return rightHand;
            case WeaponStats.ItemType.Head:
                return head;
            case WeaponStats.ItemType.Body:
                return body;
            case WeaponStats.ItemType.Feet:
                return feet;
        }

        return null;
    }

    public void Expand()
    {
        if (equipmentScreen.activeSelf)
            return;
        GetComponent<FadeScript>().FadeOut();
        leftEquip.GetComponent<HandEquip>().Expand();
        rightEquip.GetComponent<HandEquip>().Expand();
        enabled = false;
    }

    public void Contract()
    {
        if (equipmentScreen.activeSelf)
            return;
        GetComponent<FadeScript>().FadeIn();
        leftEquip.GetComponent<HandEquip>().Contract();
        rightEquip.GetComponent<HandEquip>().Contract();
        enabled = true;
    }
}

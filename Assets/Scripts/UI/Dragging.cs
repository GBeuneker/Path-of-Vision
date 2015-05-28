using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dragging : MonoBehaviour
{
    public Vector2 originalPosition;
    public Transform originalParent;
    bool isDragging;

    GameObject inspector;
    GameObject itemBar;

    // Use this for initialization
    void Start()
    {
        inspector = GameObject.Find("Inspector");
        itemBar = GameObject.Find("ItemBar");
        originalParent = transform.parent;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (itemBar.GetComponent<ItemBarBehaviour>().Out)
        {
            //Picking up behaviour
            if (Input.GetMouseButtonDown(0) && transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
            {
                StartDragging();
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                if (inspector.transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
                {
                    transform.position = inspector.transform.position;
                    inspector.GetComponent<InspectorScript>().Inspect(gameObject);
                }
                else
                {
                    transform.position = MobileInput.TouchPosition;
                    inspector.GetComponent<InspectorScript>().Clear();
                }
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                if (GameObject.Find("Equip").GetComponent<EquipScript>().collider2D.OverlapPoint(MobileInput.TouchPosition) && GameObject.Find("Equip").GetComponent<EquipScript>().enabled && isDragging)
                {
                    AutoEquipItem(gameObject);
                    StopDragging();
                }
                else if (GameObject.Find("Left").GetComponent<HandEquip>().collider2D.OverlapPoint(MobileInput.TouchPosition) && GameObject.Find("Left").GetComponent<HandEquip>().enabled && isDragging)
                {
                    AutoEquipItem(gameObject, "Left");
                    GameObject.Find("Equip").GetComponent<EquipScript>().Contract();
                    StopDragging();
                }
                else if (GameObject.Find("Right").GetComponent<HandEquip>().collider2D.OverlapPoint(MobileInput.TouchPosition) && GameObject.Find("Right").GetComponent<HandEquip>().enabled && isDragging)
                {
                    AutoEquipItem(gameObject, "Right");
                    GameObject.Find("Equip").GetComponent<EquipScript>().Contract();
                    StopDragging();
                }
                else if (CollidingItemSlot() != null)
                {
                    GameObject slot = CollidingItemSlot();
                    if(slot.GetComponent<ItemSlotScript>() != null)
                        EquipItem(gameObject, CollidingItemSlot());
                    else
                        PutInBackpack(slot);
                    StopDragging();
                }
                else
                    StopDragging();
            }
        }
    }

    GameObject CollidingItemSlot()
    {
        GameObject answer = null;
        GameObject[] slots = GameObject.FindGameObjectsWithTag("ItemSlot");
        foreach (GameObject obj in slots)
        {
            if (obj.transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
            {
                answer = obj;
                break;
            }
        }
        return answer;
    }

    void StartDragging()
    {
        transform.SetParent(GameObject.Find("UI Overlay").transform);
        originalPosition = transform.position;
        if (GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.OneHanded || GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
            GameObject.Find("Equip").GetComponent<EquipScript>().Expand();
        isDragging = true;
    }

    void StopDragging()
    {
        transform.SetParent(originalParent);
        if (originalPosition != Vector2.zero)
            transform.position = originalPosition;
        GameObject.Find("Equip").GetComponent<EquipScript>().Contract();
        isDragging = false;
        inspector.GetComponent<InspectorScript>().Clear();
    }

    void PutInBackpack(GameObject slot)
    {
        if (slot.transform.childCount > 0)
            return;
        if(originalParent.GetComponent<ItemSlotScript>() != null)
        {
            originalParent.GetComponent<ItemSlotScript>().Unequip();
        }
        transform.SetParent(slot.transform);
        originalParent = slot.transform;
        transform.position = slot.transform.position;
        originalPosition = slot.transform.position;
        isDragging = false;
    }

    void AutoEquipItem(GameObject item, string hand = "")
    {
        GameObject itemSlot = GameObject.Find("Equip").GetComponent<EquipScript>().CorrectItemSlot(item, hand);
        EquipItem(item, itemSlot);
    }

    void EquipItem(GameObject item, GameObject itemSlot)
    {
        ItemSlotScript itemSlotScript = itemSlot.GetComponent<ItemSlotScript>();

        //If the item was previously in another itemSlot and we place it in a new itemSlot
        if (originalParent.GetComponent<ItemSlotScript>() != null && itemSlot.GetComponent<ItemSlotScript>().Compatible(item))
        {
            originalParent.GetComponent<ItemSlotScript>().Unequip();
        }

        itemSlotScript.EquipItem(gameObject);

    }

    public int BackPackIndex
    {
        get;
        set;
    }
}

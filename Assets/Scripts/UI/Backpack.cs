using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{

    public GameObject itemSlider;
    public GameObject inspector;
    public GameObject equip;
    public GameObject equipmentScreen;
    public GameObject uiOverlay;

    bool isOpen;
    GameObject itemContainer;

    void Start()
    {
        equipmentScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterScript.InEncounter)
            return;

        else if (Input.GetMouseButtonDown(0))
        {
            if (transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
            {
                if (isOpen)
                    CloseBackpack();
                else
                    OpenBackpack();
            }
        }
    }

    private void OpenBackpack()
    {
        isOpen = true;
        MasterScript.stopControls = true;
        itemSlider.GetComponent<ItemBarBehaviour>().SlideOut();
        inspector.GetComponent<FadeScript>().FadeIn();
        equip.GetComponent<FadeScript>().FadeIn();
        if (itemContainer != null)
            itemContainer.SetActive(true);
        return;
    }

    private void CloseBackpack()
    {
        isOpen = false;
        MasterScript.stopControls = false;
        MasterScript.stopMovement = false;
        itemSlider.GetComponent<ItemBarBehaviour>().SlideIn();
        inspector.GetComponent<FadeScript>().FadeOut();
        equip.GetComponent<FadeScript>().FadeOut();
        equipmentScreen.SetActive(false);
        if (itemContainer != null)
            itemContainer.SetActive(false);
        return;
    }

    public GameObject ItemContainer
    {
        get { return itemContainer; }
        set
        {
            if (value != null)
            {
                itemContainer = value;
                itemContainer.SetActive(false);
                itemContainer.transform.SetParent(uiOverlay.transform);
                itemContainer.transform.localPosition = Vector2.zero;
                itemContainer.transform.localScale = Vector3.one;
            }
            else if (itemContainer != null)
            {
                itemContainer.SetActive(false);
                itemContainer.transform.SetParent(GameObject.Find("_Wastebin").transform);
                itemContainer = value;
            }
        }
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }
}

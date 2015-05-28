using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatSetup : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject secondHand;

    GameObject leftButton, rightButton;
    bool leftEquipped, rightEquipped;

    // Use this for initialization
    void Start()
    {

    }

    public void Setup(GameObject currentPlayer, CombatManager combatManager)
    {
        Equipment playerEquipment = currentPlayer.GetComponent<Equipment>();

        leftEquipped = currentPlayer.GetComponent<Equipment>().LeftHand.GetComponent<WeaponStats>().itemType != WeaponStats.ItemType.Empty;
        rightEquipped = currentPlayer.GetComponent<Equipment>().RightHand.GetComponent<WeaponStats>().itemType != WeaponStats.ItemType.Empty;

        if (leftEquipped) //If the player has a weapon in his left hand
        {
            leftButton = AddObject(attackButton, new Vector3(-260f, -450f, 0));
            if (playerEquipment.LeftHand.GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
            {
                rightButton = AddObject(secondHand, new Vector3(260f, -450f, 0));
                leftButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().LeftHand, combatManager, rightButton.GetComponent<TwoHandedButtonScript>());
            }
            else
                leftButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().LeftHand, combatManager);

        }
        if (rightEquipped) //If the player has a weapon in his right hand
        {
            rightButton = AddObject(attackButton, new Vector3(260f, -450f, 0));
            if (playerEquipment.RightHand.GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
            {
                leftButton = AddObject(secondHand, new Vector3(-260f, -450f, 0));
                rightButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().RightHand, combatManager, leftButton.GetComponent<TwoHandedButtonScript>());
            }
            else
                rightButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().RightHand, combatManager);

        }
        if (!leftEquipped && !rightEquipped) //If the player doesn't have any weapons in his hands
        {
            leftButton = AddObject(attackButton, new Vector3(-260f, -450f, 0));
            rightButton = AddObject(attackButton, new Vector3(260f, -450f, 0));

            leftButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().LeftHand, combatManager);
            rightButton.GetComponent<AttackButtonScript>().Setup(currentPlayer.GetComponent<Equipment>().RightHand, combatManager);
        }
    }

    GameObject AddObject(GameObject original, Vector3 position)
    {
        GameObject newObject = (GameObject)Instantiate(original, Vector3.zero, transform.rotation);
        newObject.transform.SetParent(transform);
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localPosition = position;
        return newObject;
    }

    public bool LeftEquipped
    {
        get { return leftEquipped; }
    }

    public bool RightEquipped
    {
        get { return rightEquipped; }
    }
}

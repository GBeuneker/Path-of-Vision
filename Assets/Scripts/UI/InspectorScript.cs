using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InspectorScript : MonoBehaviour
{

    public Text itemInfo;

    // Use this for initialization
    void Start()
    {
        itemInfo.supportRichText = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Inspect(GameObject item)
    {
        itemInfo.text = "";
        WeaponStats stats = item.GetComponent<WeaponStats>();
        itemInfo.text += "Attack: " + "<color=#930000>" + stats.minAttack.ToString() + "</color>" + "-" + "<color=#003300>" + stats.maxAttack.ToString() + "</color>";
        itemInfo.text += "\n" + "Cooldown: " + stats.cooldown.ToString() + "s";
        if(stats.defence > 0)
            itemInfo.text += "\n" + "Defence: " + "+" + stats.defence.ToString();
        if(stats.dexterity > 0)
            itemInfo.text += "\n" + "Dexterity" + "+" + stats.dexterity.ToString();
    }

    public void Clear()
    {
        itemInfo.text = "";
    }
}

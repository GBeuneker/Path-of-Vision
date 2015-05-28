using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChestBehaviour : MonoBehaviour
{
    public GameObject container;

    GameObject player;
    GameObject backpack;
    bool inRange;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        backpack = GameObject.Find("Backpack");
        inRange = false;
        container = (GameObject)Instantiate(container);
        container.transform.SetParent(GameObject.Find("_Wastebin").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= 0.1f && !inRange)
        {
            backpack.GetComponent<Backpack>().ItemContainer = container;
            backpack.GetComponent<Image>().color = Color.green;
            inRange = true;
        }
        else if ((player.transform.position - transform.position).magnitude > 0.1f && inRange)
        {
            backpack.GetComponent<Backpack>().ItemContainer = null;
            backpack.GetComponent<Image>().color = Color.white;
            inRange = false;
        }
    }


}

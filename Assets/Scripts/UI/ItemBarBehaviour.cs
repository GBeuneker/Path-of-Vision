using UnityEngine;
using System.Collections;

public class ItemBarBehaviour : MonoBehaviour
{
    float minX;
    float maxX;
    float goalX;

    public GameObject[] backpackSlots;

    // Use this for initialization
    void Start()
    {
        minX = transform.localPosition.x;
        GameObject UIOverlay = GameObject.Find("UI Overlay");
        maxX = transform.localPosition.x + GetComponent<RectTransform>().rect.width;

        goalX = minX;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(goalX - transform.localPosition.x) > 5f)
            transform.localPosition += new Vector3((goalX - transform.localPosition.x), 0, 0) * Time.deltaTime * 5f;
    }

    public void SlideOut()
    {
        goalX = maxX;
    }

    public void SlideIn()
    {
        goalX = minX;
    }

    public bool Out
    {
        get { return Mathf.Abs(maxX - transform.localPosition.x) <= 10f; }
    }

}

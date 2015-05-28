using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandEquip : MonoBehaviour
{

    public GameObject equipIcon;
    public bool enabled;

    Vector2 startPosition;
    Vector2 endPosition, goalPosition;
    Color newColor;

    // Use this for initialization
    void Start()
    {
        newColor = GetComponent<Image>().color;

        startPosition = equipIcon.transform.localPosition;
        endPosition = transform.localPosition;

        transform.localPosition = startPosition;
        goalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(goalPosition.x - transform.localPosition.x, 0, 0) * Time.deltaTime * 8f;
    }

    public void Expand()
    {
        goalPosition = endPosition;
        GetComponent<FadeScript>().FadeIn();
        enabled = true;
    }

    public void Contract()
    {
        goalPosition = startPosition;
        GetComponent<FadeScript>().FadeOut();
        enabled = false;
    }
}

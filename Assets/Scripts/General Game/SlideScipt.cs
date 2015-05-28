using UnityEngine;
using System.Collections;

public class SlideScipt : MonoBehaviour
{
    public Vector3 slideOffset;
    Vector3 originalPosition;
    Vector3 goalPosition;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.localPosition;
        transform.localPosition = originalPosition + slideOffset;
        goalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if ((goalPosition - transform.localPosition).magnitude > 5f)
            transform.localPosition += (goalPosition - transform.localPosition) * Time.deltaTime * 5f;
    }

    public void SlideIn()
    {
        goalPosition = originalPosition;
    }

    public void SlideOut()
    {
        goalPosition = originalPosition + slideOffset;
    }
}

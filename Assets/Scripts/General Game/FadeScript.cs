using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    Color newColor;
    bool fadeIn;

    // Use this for initialization
    void Start()
    {
        newColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (newColor.a < 1)
            if (fadeIn)
            {
                newColor.a += Time.deltaTime * 3.5f;
                GetComponent<Image>().color = newColor;
            }

        if (newColor.a > 0)
            if (!fadeIn)
            {
                newColor.a -= Time.deltaTime * 3.5f;
                GetComponent<Image>().color = newColor;
            }

    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeIn = false;
    }
}

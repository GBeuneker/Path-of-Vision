using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwoHandedButtonScript : MonoBehaviour
{
    bool pressed;
    public Image icon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (collider2D.OverlapPoint(MobileInput.TouchPosition) && !Pressed)
                Pressed = true;
        }
        else
            Pressed = false;
    }

    public bool Pressed
    {
        get { return pressed; }
        set
        {
            if (value == true)
            {
                GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f);
                icon.color = new Color(0.15f, 0.15f, 0.15f);
            }
            else
            {
                GetComponent<Image>().color = Color.white;
                icon.color = Color.white;
            }
            pressed = value;
        }
    }
}

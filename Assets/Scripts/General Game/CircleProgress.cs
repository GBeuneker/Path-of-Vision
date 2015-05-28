using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleProgress : MonoBehaviour
{

    public Image CircleImage;
    public Color start;
    public Color end;
    public Color current;

    public float timeToComplete;
    float value;

    void Start()
    {
        CircleImage.type = Image.Type.Filled;
        CircleImage.fillMethod = Image.FillMethod.Radial360;
        CircleImage.fillOrigin = 0;
        value = 1;
    }

    void Update()
    {
        float rate = 1 / timeToComplete;
        if (value > 0)
            value -= Time.deltaTime * rate;

        CircleImage.fillAmount = value;
        CircleImage.color = Color.Lerp(end, start, CircleImage.fillAmount);
        current = CircleImage.color;
    }

    public float TimeToComplete
    {
        get { return timeToComplete; }
        set
        {
            timeToComplete = value;
            this.value = 1;
        }
    }
}

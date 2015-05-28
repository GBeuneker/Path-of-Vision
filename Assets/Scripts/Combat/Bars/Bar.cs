using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class Bar
{
 protected RectTransform barTransform;

    protected float cachedY;
    protected float minXValue, maxXValue;
    protected float currentValue;

    protected GameObject attachedObject;
    protected Text barText;

    public Bar(RectTransform barTransform, GameObject attachedObject, Text barText)
    {
        this.barTransform = barTransform;

        cachedY = barTransform.localPosition.y;
        maxXValue = barTransform.localPosition.x;
        minXValue = barTransform.localPosition.x - barTransform.rect.width;

        this.OriginalPosition = barTransform.localPosition;

        this.attachedObject = attachedObject;
        this.barText = barText;
    }

    public virtual void Update()
    {
        HandleBarValues();
    }

    protected virtual void HandleBarValues()
    {

    }

    protected float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public Vector2 OriginalPosition
    {
        get;
        set;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Bar
{
    public HealthBar(RectTransform healthTransform, GameObject attachedObject, Text healthText)
        : base(healthTransform, attachedObject, healthText)
    {
        currentValue = attachedObject.GetComponent<Stats>().currentHealth;
        HandleBarValues();
    }

    public override void Update()
    {
        if (Mathf.Abs(attachedObject.GetComponent<Stats>().currentHealth - this.currentValue) > 0.1f)
        {
            Stats attachedStats = attachedObject.GetComponent<Stats>();
            float difference = (this.currentValue - attachedStats.currentHealth) / 5;
            attachedStats.currentHealth = Mathf.Clamp(attachedStats.currentHealth + difference, currentValue, attachedStats.maxHealth);
            float test = attachedStats.currentHealth;
            HandleBarValues();
        }

    }

    protected override void HandleBarValues()
    {
        Stats attachedStats = attachedObject.GetComponent<Stats>();

        float currentXValuePlayer = MapValues(attachedStats.currentHealth, 0,
                                              attachedStats.maxHealth, minXValue, maxXValue);
        barTransform.localPosition = new Vector3(currentXValuePlayer, cachedY);

        barText.text = ((int)attachedStats.currentHealth).ToString() + "/" + ((int)attachedStats.maxHealth).ToString();

    }

    public float CurrentHealth
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
        }
    }

    public bool Done
    {
        get { return Mathf.Abs(attachedObject.GetComponent<Stats>().currentHealth - this.currentValue) <= 0.1f; }
    }
}


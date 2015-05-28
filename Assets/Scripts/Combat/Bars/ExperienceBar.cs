using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : Bar
{
    float delay;
    public bool pause;
    CombatManager combat;

    public ExperienceBar(RectTransform barTransform, GameObject attachedObject, Text barText, CombatManager combat)
        :base(barTransform, attachedObject, barText)
    {
        this.combat = combat;

        currentValue = attachedObject.GetComponent<Player>().Experience;
        HandleBarValues();
    }

    public override void Update()
    {
        if (pause)
            return;
        delay += Time.deltaTime;

        if(delay >= 0.05f)
        {
            delay = 0;
            if (this.currentValue != attachedObject.GetComponent<Player>().Experience)
            {
                this.currentValue += 1;
                if (this.currentValue >= attachedObject.GetComponent<Player>().maxExperience)
                {
                    int test = attachedObject.GetComponent<Player>().maxExperience;
                    this.currentValue = 0;
                    attachedObject.GetComponent<Player>().LevelUp();
                    combat.LevelUp();
                    pause = true;
                }

                HandleBarValues();
            }
        }
    }

    protected override void HandleBarValues()
    {
        Player playerInfo = attachedObject.GetComponent<Player>();

        float currentXValuePlayer = MapValues(currentValue, 0,
                                              playerInfo.maxExperience, minXValue, maxXValue);

        barTransform.localPosition = new Vector3(currentXValuePlayer, cachedY);

        barText.text = ((int)currentValue).ToString() + "/" + ((int)playerInfo.maxExperience).ToString() + " xp";

    }

    public bool Done
    {
        get { return this.currentValue >= attachedObject.GetComponent<Player>().Experience; }
    }
}


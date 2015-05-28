using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool hasKey;
    public Sprite combatSprite;

    int experience;
    public int maxExperience;

    float delay;

    // Use this for initialization
    void Start()
    {
        this.experience = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelUp()
    {
        this.Experience -= this.maxExperience;
        this.maxExperience += Mathf.RoundToInt(maxExperience * 0.25f);
    }

    public int Experience
    {
        get { return experience; }
        set
        {
            experience = value;
        }
    }
}

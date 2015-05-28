using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{

    public float currentHealth;
    public float maxHealth;

    //Attack stats
    public float accuracy_player;
    public int defence;
    public float dexterity;
    public int intelligence;
    public int strength;
    public float attackDelay_enemy;
    public float attackSpeed_enemy;
    public int exp_enemy;

    void Start()
    {
        CurrentIntelligence = intelligence;
        CurrentAccuracy = accuracy_player;
        CurrentDefence = defence;
        CurrentDexterity = dexterity;
        CurrentMaxHealth = maxHealth;
    }

    public float Defence
    {
        get { return 100f / (100 + CurrentDefence); }
    }


    public int CurrentIntelligence
    {
        get;
        set;
    }
    public float CurrentAccuracy
    {
        get;
        set;
    }
    public int CurrentDefence
    {
        get;
        set;
    }
    public float CurrentDexterity
    {
        get;
        set;
    }
    public float CurrentMaxHealth
    {
        get;
        set;
    }


}

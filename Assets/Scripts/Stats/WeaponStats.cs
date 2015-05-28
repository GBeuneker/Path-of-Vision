using UnityEngine;
using System.Collections;

public class WeaponStats : MonoBehaviour
{
    public enum ItemType
    {
        OneHanded, TwoHanded, Head, Body, Feet, Empty
    }

    public ItemType itemType;
    public string name;
    public float maxHealth;

    //Attack stats
    public int minAttack, maxAttack;
    public float cooldown;
    public int intelligence;
    public float swiftChance;

    //Defense stats
    public int defence;
    public float dexterity;

    // Use this for initialization
    void Start()
    {

    }

    public int Attack
    {
        get { return Random.Range(minAttack, maxAttack + 1); }
    }
}

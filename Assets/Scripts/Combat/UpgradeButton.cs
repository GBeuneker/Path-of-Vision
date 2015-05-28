using UnityEngine;
using System.Collections;

public class UpgradeButton : MonoBehaviour
{
    public enum Type { Health, Strength, Intelligence, Acuracy, Defence, Dexterity }
    public Type upgradeType;
    public GameObject upgradeTree;

    Stats playerStats;
    CombatManager combat;

    // Use this for initialization
    void Start()
    {
        combat = GameObject.FindWithTag("CombatOverlay").GetComponent<CombatManager>();
        playerStats = combat.PlayerStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.collider2D.OverlapPoint(MobileInput.TouchPosition))
            {
                UpgradeSkill();
            }

        }
    }

    void UpgradeSkill()
    {
        switch (upgradeType)
        {
            case Type.Health:
                playerStats.maxHealth += 10;
                break;
            case Type.Strength:
                playerStats.strength += 10;
                break;
            case Type.Intelligence:
                playerStats.intelligence += 10;
                break;
            case Type.Acuracy:
                playerStats.accuracy_player += 10;
                break;
            case Type.Defence:
                playerStats.defence += 10;
                break;
            case Type.Dexterity:
                playerStats.dexterity += 10;
                break;
            default:
                break;
        }

        if(combat != null)
        {
            combat.ExperienceBar.pause = false;
        }
        Destroy(upgradeTree.gameObject);
        combat.GetComponent<CombatManager>().LeveledUp = false;
    }
}

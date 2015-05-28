using UnityEngine;
using System.Collections;
using System;

public class EnemyAttack : MonoBehaviour
{
    HealthBar playerHealth;
    GameObject combatOverlay;
    Stats stats;
    public Sprite combatSprite;
    public float basicAttackChance, sliceAttackChance, poisonAttackChance;

    float attackTimer;
    float delay;

    // Use this for initialization
    void Start()
    {
        this.stats = GetComponent<Stats>();
        if (stats == null)
            throw new Exception("No stats attached to enemy object!");
        delay = stats.attackDelay_enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatOverlay != null)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= delay)
            {
                float attack = UnityEngine.Random.Range(0, 100);
                if (attack < basicAttackChance) //basicAtkChance% of the time this attack is executed
                {
                    BasicAttack();
                }
                else if (attack < sliceAttackChance + basicAttackChance) //sliceAtkChance% of the time this attack is executed
                {
                    SliceAttack();
                }
                else if (attack < sliceAttackChance + basicAttackChance + poisonAttackChance) //poisonAtkChance% of the time this attack is executed
                {
                    PoisonAttack();
                }

                attackTimer = 0;
                delay = UnityEngine.Random.Range(0.2f, 1.7f) * stats.attackDelay_enemy; //Randomly vary from the attack delay
            }

        }
    }

    public void Setup(HealthBar playerHealth, GameObject combatOverlay)
    {
        this.playerHealth = playerHealth;
        this.combatOverlay = combatOverlay;
    }

    public void BasicAttack()
    {
        GameObject shield = Resources.Load<GameObject>("Combat Prefabs/Shield");
        Vector3 scale = shield.transform.localScale;

        shield = (GameObject)Instantiate(shield, combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer, transform.rotation);
        shield.transform.SetParent(combatOverlay.transform);
        shield.transform.localScale = scale;

        shield.GetComponent<ShieldScript>().Setup(playerHealth, gameObject, combatOverlay);
    }

    public void SliceAttack()
    {
        Vector2 sliceStart = combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer;

        GameObject slice = Resources.Load<GameObject>("Effect Prefabs/Slice");
        slice = (GameObject)Instantiate(slice, new Vector3(sliceStart.x, sliceStart.y, slice.transform.position.z), transform.rotation);
        slice.GetComponent<SliceScript>().Setup(playerHealth, gameObject, combatOverlay);
    }

    public void PoisonAttack()
    {
        GameObject poison = Resources.Load<GameObject>("Combat Prefabs/Poison");
        Vector3 scale = poison.transform.localScale;

        poison = (GameObject)Instantiate(poison, combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer, transform.rotation);
        poison.transform.SetParent(combatOverlay.transform);
        poison.transform.localScale = scale;

        poison.GetComponent<PoisonScript>().Setup(playerHealth, gameObject, combatOverlay);
    }

    float BasicAttackChance
    {
        get { return basicAttackChance; }
    }

    float SliceAttackChance
    {
        get { return basicAttackChance + sliceAttackChance; }
    }
}

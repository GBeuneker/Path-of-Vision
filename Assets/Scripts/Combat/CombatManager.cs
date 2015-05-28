using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    public GameObject playerSprite;
    public GameObject enemySprite;

    GameObject currentEnemy;
    GameObject currentPlayer;

    //Health bars
    public GameObject experienceBarObject;
    public RectTransform healthTransformPlayer, healthTransformEnemy, experienceBarTransform;
    public Text healthPlayerText, healthEnemyText, experienceBarText;
    HealthBar playerHealth, enemyHealth;
    ExperienceBar experienceBar;

    public bool finishedFight;
    float destroyDelay;

    // Use this for initialization
    void Start()
    {
        MasterScript.InEncounter = true;
        finishedFight = false;
    }

    public void Setup(GameObject currentPlayer, GameObject currentEnemy)
    {
        this.currentEnemy = currentEnemy;
        this.currentPlayer = currentPlayer;

        enemySprite.GetComponent<Image>().sprite = currentEnemy.GetComponent<EnemyAttack>().combatSprite;
        playerSprite.GetComponent<Image>().sprite = currentPlayer.GetComponent<Player>().combatSprite;

        playerHealth = new HealthBar(healthTransformPlayer, currentPlayer, healthPlayerText);
        enemyHealth = new HealthBar(healthTransformEnemy, currentEnemy, healthEnemyText);
        experienceBar = new ExperienceBar(experienceBarTransform, currentPlayer, experienceBarText, this);

        GetComponent<CombatSetup>().Setup(currentPlayer, this);
        currentEnemy.GetComponent<EnemyAttack>().Setup(playerHealth, gameObject);

        GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("Effects Camera").camera;
    }

    void Update()
    {
        experienceBar.Update();

        if (finishedFight)
        {
            if (LeveledUp)
                return;

            if (experienceBar.Done)
                destroyDelay += Time.deltaTime;
            if (destroyDelay >= 1.0f && !LeveledUp)
            {
                Destroy(gameObject);
                MasterScript.InEncounter = false;
            }

            return;
        }
        playerHealth.Update();
        enemyHealth.Update();

        if (currentEnemy.GetComponent<Stats>().currentHealth < 1)
            DefeatEnemy();
        if (currentPlayer.GetComponent<Stats>().currentHealth < 1)
            Die();
    }

    public void AttackEnemy(GameObject weapon)
    {
        Stats playerStats = currentPlayer.GetComponent<Stats>();
        Stats enemyStats = currentEnemy.GetComponent<Stats>();

        if (UnityEngine.Random.Range(0, 100f) <= playerStats.accuracy_player || CriticalHit)
        {
            //Hit the enemy
            int damage = Mathf.RoundToInt((float)(playerStats.strength + weapon.GetComponent<WeaponStats>().Attack) * enemyStats.Defence);
            if (CriticalHit)
                damage *= 2;
            ShowEffect("Combat Prefabs/Blood", damage.ToString());
            enemyHealth.CurrentHealth -= damage;
        }
        else
        {
            //Miss the enemy
            ShowEffect("Combat Prefabs/Miss", "Miss");
        }
    }

    private void DefeatEnemy()
    {
        currentPlayer.GetComponent<RoomDrawing>().KillEnemy(currentEnemy);
        GainExperience();
    }

    private void GainExperience()
    {
        currentPlayer.GetComponent<Player>().Experience += currentEnemy.GetComponent<Stats>().exp_enemy;
        experienceBarObject.GetComponent<SlideScipt>().SlideIn();
        finishedFight = true;
    }

    public void LevelUp()
    {
        LeveledUp = true;
        destroyDelay = 0.5f;
        GameObject upgradeFlower = Resources.Load<GameObject>("Combat Prefabs/UpgradeFlower");
        upgradeFlower = (GameObject)GameObject.Instantiate(upgradeFlower);
        upgradeFlower.transform.SetParent(transform);
        upgradeFlower.transform.localScale = Vector3.one;
        upgradeFlower.transform.localPosition = Vector2.zero;
    }

    private void Die()
    {
        Application.LoadLevel("Main Game");
    }

    private void ShowEffect(string path, string text)
    {
        GameObject effect = Resources.Load<GameObject>(path);
        Vector3 scale = effect.transform.localScale;
        EffectBehaviour effectBehaviour = effect.GetComponent<EffectBehaviour>();

        if (effectBehaviour == null)
            throw new Exception("Effect not found (Error 005)");

        effectBehaviour.Initialize(text);
        effect = (GameObject)Instantiate(effect, RandomAttackPositionEnemy, transform.rotation);
        effect.transform.SetParent(transform);
        effect.transform.localScale = scale;

    }

    public bool WithinPlayerBoundaries(Vector2 input)
    {
        Vector2 spriteSize = new Vector2(playerSprite.GetComponent<RectTransform>().sizeDelta.x * GetComponent<RectTransform>().localScale.x,
                                         playerSprite.GetComponent<RectTransform>().sizeDelta.y * GetComponent<RectTransform>().localScale.y);

        if (input.x < playerSprite.transform.position.x - spriteSize.x / 2 || input.x > playerSprite.transform.position.x + spriteSize.x / 2)
            return false;
        if (input.y < playerSprite.transform.position.y - spriteSize.y / 2 || input.y > playerSprite.transform.position.y + spriteSize.y / 2)
            return false;

        return true;
    }

    public Vector2 UIScale
    {
        get { return GetComponent<RectTransform>().localScale; }
    }

    public ExperienceBar ExperienceBar
    {
        get { return experienceBar; }
    }

    public Stats PlayerStats
    {
        get { return currentPlayer.GetComponent<Stats>(); }
    }

    public Stats EnemyStats
    {
        get { return currentEnemy.GetComponent<Stats>(); }
    }

    public Vector3 RandomAttackPositionPlayer
    {
        get
        {
            Vector2 spriteSize = new Vector2(playerSprite.GetComponent<RectTransform>().sizeDelta.x * GetComponent<RectTransform>().localScale.x,
                                            playerSprite.GetComponent<RectTransform>().sizeDelta.y * GetComponent<RectTransform>().localScale.y);

            bool collision = true;
            float randomX = 0, randomY = 0;
            int tries = 0;
            //Make sure our random point does not intersect with any other clickable objects
            while (collision && tries < 500)
            {
                tries++;
                randomX = UnityEngine.Random.Range(playerSprite.transform.position.x - spriteSize.x / 2,
                                                   playerSprite.transform.position.x + spriteSize.x / 2);
                randomY = UnityEngine.Random.Range(playerSprite.transform.position.y - spriteSize.y / 2,
                                                   playerSprite.transform.position.y + spriteSize.y / 2);

                collision = false;
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Clickable Object"))
                {
                    if (g.collider2D.OverlapPoint(new Vector2(randomX, randomY)))
                        collision = true;
                }

            }
            return new Vector3(randomX, randomY, 0);
        }
    }

    public Vector3 RandomAttackPositionEnemy
    {
        get
        {
            Vector2 spriteSize = new Vector2(enemySprite.GetComponent<RectTransform>().sizeDelta.x * GetComponent<RectTransform>().localScale.x,
                                             enemySprite.GetComponent<RectTransform>().sizeDelta.y * GetComponent<RectTransform>().localScale.y);

            float randomX = UnityEngine.Random.Range(enemySprite.transform.position.x - spriteSize.x / 2,
                                                     enemySprite.transform.position.x + spriteSize.x / 2);
            float randomY = UnityEngine.Random.Range(enemySprite.transform.position.y - spriteSize.y / 2,
                                                     enemySprite.transform.position.y + spriteSize.y / 2);

            return new Vector3(randomX, randomY, 0);
        }
    }

    public bool LeveledUp
    {
        get;
        set;
    }

    public bool CriticalHit
    {
        get;
        set;
    }

    public float CritChance
    {
        get
        {
            WeaponStats leftWeapon = currentPlayer.GetComponent<Equipment>().LeftHand.GetComponent<WeaponStats>();
            WeaponStats rightWeapon = currentPlayer.GetComponent<Equipment>().RightHand.GetComponent<WeaponStats>();

            if (GetComponent<CombatSetup>().LeftEquipped && GetComponent<CombatSetup>().RightEquipped)
            {
                //Return the average of both crit chances
                return (leftWeapon.swiftChance + rightWeapon.swiftChance) / 2;
            }
            else if (GetComponent<CombatSetup>().LeftEquipped)
                return leftWeapon.swiftChance;
            else if (GetComponent<CombatSetup>().RightEquipped)
                return rightWeapon.swiftChance;
            else
                return 0;
        }
    }
}

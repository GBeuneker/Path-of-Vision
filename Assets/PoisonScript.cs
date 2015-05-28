using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PoisonScript : MonoBehaviour
{
    public Text timeLeft;
    public float timer;
    public GameObject radialTimer;

    HealthBar playerHealth;
    List<GameObject> shieldList;

    GameObject player, enemy;
    GameObject combatOverlay;
    Color imageColor;

    // Use this for initialization
    void Start()
    {
        imageColor = GetComponent<Image>().color;
    }

    public void Setup(HealthBar playerHealth, GameObject enemy, GameObject combatOverlay)
    {
        this.enemy = enemy;
        this.combatOverlay = combatOverlay;
        this.playerHealth = playerHealth;

        timer = 3;
        radialTimer.GetComponent<CircleProgress>().timeToComplete = timer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timeLeft.text = timer.ToString("F1");

        if (timer <= 0)
            Block();
        imageColor.a = timer / 3;
        GetComponent<Image>().color = imageColor;

        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                if (collider2D.OverlapPoint(MobileInput.GetWorldPosition(t.position)))
                    Damage();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (collider2D.OverlapPoint(MobileInput.GetWorldPosition(Input.mousePosition)))
                Damage();
        }
    }

    public void Block()
    {
        Destroy(gameObject);
    }

    void Damage()
    {
        Stats enemyStats = combatOverlay.GetComponent<CombatManager>().EnemyStats;
        Stats playerStats = combatOverlay.GetComponent<CombatManager>().PlayerStats;

        int damage = Mathf.RoundToInt((float)enemyStats.strength * playerStats.Defence);
        playerHealth.CurrentHealth -= damage;
        ShowEffect("Combat Prefabs/Blood", damage.ToString(), combatOverlay.GetComponent<CombatManager>().playerSprite);
        Destroy(gameObject);
    }

    private void ShowEffect(string path, string text, GameObject target)
    {
        GameObject effect = Resources.Load<GameObject>(path);
        Vector3 scale = effect.transform.localScale;

        EffectBehaviour effectBehaviour = effect.GetComponent<EffectBehaviour>();
        if (effectBehaviour == null)
            throw new Exception("Effect not found (Error 005)");

        effectBehaviour.Initialize(text);
        effect = (GameObject)Instantiate(effect, combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer, transform.rotation);
        effect.transform.SetParent(combatOverlay.transform);
        effect.transform.localScale = scale;

    }
}

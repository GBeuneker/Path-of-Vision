using UnityEngine;
using System.Collections;
using System;

public class SliceScript : MonoBehaviour
{
    Vector3 startPos, endPos;
    public float clearence;
    public float blockTime;
    float timer;

    HealthBar playerHealth;
    GameObject enemy, combatOverlay;
    bool blocked;

    // Use this for initialization
    void Start()
    {

    }

    public void Setup(HealthBar playerHealth, GameObject enemy, GameObject combatOverlay)
    {
        Vector2 sliceStart = combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer;
        Vector2 sliceEnd = combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer;
        while ((sliceStart - sliceEnd).magnitude < 2)
        {
            sliceEnd = combatOverlay.GetComponent<CombatManager>().RandomAttackPositionPlayer;
        }

        this.startPos = new Vector3(sliceStart.x, sliceStart.y, transform.position.z);
        this.endPos = new Vector3(sliceEnd.x, sliceEnd.y, transform.position.z);
        transform.position = startPos;

        GetComponent<MoveToDestination>().MoveTo(this.endPos, 8, true);

        this.playerHealth = playerHealth;
        this.enemy = enemy;
        this.combatOverlay = combatOverlay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            foreach(Touch t in Input.touches)
            {
                if (InBetween(startPos, endPos, MobileInput.GetWorldPosition(t.position)))
                    Block();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (InBetween(startPos, endPos, MobileInput.GetWorldPosition(Input.mousePosition)))
                Block();
        }
        if((transform.position - endPos).magnitude <= 0.1f)
        {
            timer += Time.deltaTime;
            if (timer > blockTime)
                Damage();
        }
    }

    void Block()
    {
        blocked = true;
        Vector2 position = MobileInput.GetWorldPosition(Input.mousePosition);
        GameObject sparks = Resources.Load<GameObject>("Effect Prefabs/Sparks");
        Instantiate(sparks, position, transform.rotation);
        Destroy(gameObject);
    }

    void Damage()
    {
        if (blocked)
        {
            Destroy(gameObject);
            return;
        }

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

    float Distance(Vector2 a, Vector2 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    bool InBetween(Vector2 a, Vector2 b, Vector2 input)
    {
        //Check is input is between a and b with a bit of clearence
        float toCheck = Distance(a, b);
        float dist1 = (Distance(a, input) + Distance(input, b));
        float dist2 = (Distance(a, input) + Distance(input, b));

        return (Distance(a, input) + Distance(input, b)) >= Distance(a, b) - clearence && (Distance(a, input) + Distance(input, b)) <= Distance(a, b) + clearence;
    }
}

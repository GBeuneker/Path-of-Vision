using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackButtonScript : MonoBehaviour
{
    public Image weaponImage;
    GameObject weapon;
    CombatManager combatManager;
    TwoHandedButtonScript twoHanded;
    float attackTimer;

    // Use this for initialization
    void Start()
    {
        attackTimer = 0;
        GetComponentInChildren<CircleProgress>().TimeToComplete = attackTimer;
    }

    public void Setup(GameObject weapon, CombatManager combatManager, TwoHandedButtonScript twoHanded = null)
    {
        gameObject.SetActive(true);
        this.weapon = weapon;
        this.combatManager = combatManager;
        weaponImage.GetComponent<Image>().sprite = weapon.GetComponent<Image>().sprite;
        if (twoHanded != null)
            this.twoHanded = twoHanded;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0)
        {
            if (weapon.GetComponent<WeaponStats>().itemType == WeaponStats.ItemType.TwoHanded)
            {
                if (Input.touchCount > 1)
                {
                    Touch touch = Input.touches[1];
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (collider2D.OverlapPoint(MobileInput.TouchPosition) && twoHanded.Pressed)
                            Attack();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (collider2D.OverlapPoint(MobileInput.TouchPosition))
                        Attack();
                }
            }
        }
    }

    public void Attack()
    {
        if (combatManager.finishedFight)
            return;
        if (Random.Range(0, 100) < weapon.GetComponent<WeaponStats>().swiftChance)
        {
            attackTimer = 0.5f;
            SwiftStrike();
        }
        else
            attackTimer = weapon.GetComponent<WeaponStats>().cooldown;

        GetComponentInChildren<CircleProgress>().TimeToComplete = attackTimer;
        combatManager.AttackEnemy(weapon);
    }

    public void SwiftStrike()
    {
        GameObject effect = Resources.Load<GameObject>("Effect Prefabs/Swift Strike");
        effect = (GameObject)Instantiate(effect, Vector3.zero, transform.rotation);
        effect.transform.SetParent(transform.parent.transform);
        effect.transform.localScale = Vector3.one;
        effect.transform.localPosition = Vector3.one;
    }
}

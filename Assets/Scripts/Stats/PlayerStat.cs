using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    
    public GameObject player;

    public enum PlayerStats
    {
        Accuracy, Defence, Dexterity, Health, Intelligence, Strength  
    }
    public PlayerStats statToFollow;

    Text currentText;
    string originalString;

    // Use this for initialization
    void Start()
    {
        currentText = GetComponent<Text>();
        originalString = currentText.text;
    }

    // Update is called once per frame
    void Update()
    {
        Stats playerStats = player.GetComponent<Stats>();
        
        switch(statToFollow)
        {
            case PlayerStats.Accuracy:
                currentText.text = originalString + playerStats.accuracy_player.ToString() + "%";
                break;
            case PlayerStats.Strength:
                currentText.text = originalString + playerStats.strength.ToString();
                break;
            case PlayerStats.Intelligence:
                currentText.text = originalString + playerStats.CurrentIntelligence.ToString();
                break;
            case PlayerStats.Defence:
                currentText.text = originalString + playerStats.CurrentDefence.ToString();
                break;
            case PlayerStats.Dexterity:
                currentText.text = originalString + playerStats.CurrentDexterity.ToString();
                break;
            case PlayerStats.Health:
                currentText.text = originalString + Mathf.RoundToInt(playerStats.currentHealth).ToString() + "/" + playerStats.CurrentMaxHealth.ToString();
                break;
            default:
                break;
        }
    }
}

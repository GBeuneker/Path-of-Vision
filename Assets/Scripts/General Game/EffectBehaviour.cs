using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class EffectBehaviour : MonoBehaviour
{

    public Text currentText;

    float timeToLive;

    // Use this for initialization
    void Start()
    {
        timeToLive = 2;
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(string text)
    {
        currentText.text = text;
    }
}

using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    public float destroyTimer;
    float timer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTimer)
            Destroy(gameObject);
    }
}

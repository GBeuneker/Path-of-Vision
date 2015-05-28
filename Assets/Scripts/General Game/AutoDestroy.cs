using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{

    ParticleSystem ps;

    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
                Destroy(gameObject);
            if (ps.particleCount <= 0 && ps.enableEmission == false)
                Destroy(gameObject);
        }
    }
}

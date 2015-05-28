using UnityEngine;
using System.Collections;

public class MoveToDestination : MonoBehaviour
{
    Vector3 destination;
    bool accelerated;
    float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (accelerated && (transform.position - destination).magnitude > 0.1f)
        {
            transform.position += Time.deltaTime * (destination - transform.position) * speed;
        }
        else if ((transform.position - destination).magnitude > Time.deltaTime * speed)
        {
            transform.position += Time.deltaTime * new Vector3((destination.x - transform.position.x) * speed, (destination.y - transform.position.y) * speed, 0);
        }
    }

    public void MoveTo(Vector2 position, float speed, bool accelerated = false)
    {
        destination = new Vector3(position.x, position.y, transform.position.z);
        this.accelerated = accelerated;
        this.speed = speed;
    }
}

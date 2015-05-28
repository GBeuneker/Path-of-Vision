using UnityEngine;
using System.Collections;

public class MobileInput : MonoBehaviour
{

    private Vector2 startPos;
    public float minSwipeDistance;
    static Camera effectCamera;

    public enum Direction
    {
        None, Left, Right, Up, Down
    }
    public static Direction swipeDirection;

    // Update is called once per frame
    void Update()
    {
        if (MasterScript.stopControls)
        {
            swipeDirection = Direction.None;
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    float angle = getAngle(startPos, touch.position);
                    float distance = (startPos - touch.position).magnitude;
                    if (distance > minSwipeDistance)
                    {
                        if (angle >= 45 && angle < 135)
                            swipeDirection = Direction.Down;
                        else if (angle >= 135 && angle < 225)
                            swipeDirection = Direction.Right;
                        else if (angle >= 225 && angle < 315)
                            swipeDirection = Direction.Up;
                        else
                            swipeDirection = Direction.Left;
                    }
                    break;
            }
        }
        else
            swipeDirection = Direction.None;

    }

    private float getAngle(Vector2 pointA, Vector2 pointB)
    {
        Vector2 newVector = pointA - pointB;
        float answer = Mathf.Atan2(newVector.y, newVector.x) * Mathf.Rad2Deg;
        if (answer < 0)
            return answer + 360;
        else
            return answer;
    }

    public static Vector2 GetWorldPosition(Vector2 input)
    {
        if (effectCamera == null)
            effectCamera = GameObject.Find("Effect Camera").camera;
        return effectCamera.ScreenToWorldPoint(input);
    }

    public static Vector2 TouchPosition
    {
        get
        {
            if (Input.touchCount > 0)
                return GameObject.FindWithTag("Effects Camera").camera.ScreenToWorldPoint(Input.touches[0].position);
            else
                return GameObject.FindWithTag("Effects Camera").camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}

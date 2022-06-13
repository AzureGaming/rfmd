using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public delegate void SwipeUp();
    public static SwipeUp OnSwipeUp;
    public delegate void SwipeDown();
    public static SwipeDown OnSwipeDown;
    int pixelOffset = 20;
    bool isFingerDown;
    Vector2 startPos;

    private void Update()
    {
        if (!isFingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            isFingerDown = true;
        }

        if (isFingerDown)
        {
            if (Input.touches.Length < 1 || Input.touches[0].phase == TouchPhase.Ended)
            {
                isFingerDown = false;
            }

            if (Input.touches[0].position.y >= startPos.y + pixelOffset)
            {
                isFingerDown = false;
                OnSwipeUp?.Invoke();
            }

            if (Input.touches[0].position.y <= startPos.y - pixelOffset)
            {
                isFingerDown = false;
                OnSwipeDown?.Invoke();
            }
        }
    }
}

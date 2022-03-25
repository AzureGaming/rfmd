using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private void OnEnable()
    {
        GameManager.OnDifficultyUp += HandleDifficultyUp;
    }

    private void OnDisable()
    {
        GameManager.OnDifficultyUp -= HandleDifficultyUp;
    }
    private void Update()
    {
        Vector3 dir = transform.right;
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public IEnumerator Stop()
    {
        float totalTime = 2f;
        float timeElapsed = 0f;
        while (timeElapsed <= totalTime || speed > 0f)
        {
            if (speed > 0f)
            {
                speed -= 0.001f;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    void HandleDifficultyUp(int level)
    {
        switch (level)
        {
            case 1:
                speed = GameManager.CAMERA_SPEED_LEVEL_1;
                break;
            case 2:
                speed = GameManager.CAMERA_SPEED_LEVEL_2;
                break;
            case 3:
                speed = GameManager.CAMERA_SPEED_LEVEL_3;
                break;
            default:
                speed = GameManager.CAMERA_SPEED_LEVEL_0;
                break;
        }
    }
}

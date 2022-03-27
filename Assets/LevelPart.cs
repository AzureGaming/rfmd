
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : Translate
{
    const float LEVEL_PART_SPEED_LEVEL_0 = 1.75f;
    const float LEVEL_PART_SPEED_LEVEL_1 = 3f;

    private void OnEnable()
    {
        GameManager.OnDifficultyUp += SetSpeed;
    }

    private void OnDisable()
    {
        GameManager.OnDifficultyUp -= SetSpeed;
    }

    void SetSpeed(int level)
    {
        switch (level)
        {
            case 0:
                    speed = LEVEL_PART_SPEED_LEVEL_0;
                break;
            case 1:
                speed = LEVEL_PART_SPEED_LEVEL_1;
                break;
            default:
                break;
        }
    }
}

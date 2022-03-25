 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpeed : MonoBehaviour
{
    [SerializeField] ParallaxBackground frontRocks;
    [SerializeField] ParallaxBackground backRocks;
    [SerializeField] ParallaxBackground mountains;
    [SerializeField] ParallaxBackground background;
    [SerializeField] ParallaxBackground ground;

    private void OnEnable()
    {
        GameManager.OnDifficultyUp += HandleDifficultyUp;
    }

    private void OnDisable()
    {
        GameManager.OnDifficultyUp -= HandleDifficultyUp;
    }

    void HandleDifficultyUp(int level)
    {
        switch (level)
        {
            case 1:
                frontRocks.parallax = GameManager.PARALLAX_FRONT_ROCKS_LEVEL_1;
                backRocks.parallax = GameManager.PARALLAX_BACK_ROCKS_LEVEL_1;
                mountains.parallax = GameManager.PARALLAX_MOUNTAINS_LEVEL_1;
                background.parallax = GameManager.PARALLAX_BACKGROUND_LEVEL_1;
                ground.parallax = GameManager.PARALLAX_GROUND_LEVEL_1;
                break;
            default:
                frontRocks.parallax = GameManager.PARALLAX_FRONT_ROCKS_LEVEL_0;
                backRocks.parallax = GameManager.PARALLAX_BACK_ROCKS_LEVEL_0;
                mountains.parallax = GameManager.PARALLAX_MOUNTAINS_LEVEL_0;
                background.parallax = GameManager.PARALLAX_BACKGROUND_LEVEL_0;
                ground.parallax = GameManager.PARALLAX_GROUND_LEVEL_0;
                break;
        }
    }
}

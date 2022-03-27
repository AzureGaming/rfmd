using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpeed : MonoBehaviour
{
    const string GROUND_TAG = "BG_Ground";
    const string BACKGROUND_TAG = "BG_Background";
    const string MOUNTAINS_TAG = "BG_Mountains";
    const string BACK_ROCKS_TAG = "BG_Back_Rocks";
    const string FRONT_ROCKS_TAG = "BG_Front_Rocks";


    const float FRONT_ROCKS_SPEED_LEVEL_0 = 1f;
    const float BACK_ROCKS_SPEED_LEVEL_0 = 1f;
    const float MOUNTAINS_SPEED_LEVEL_0 = 0.3f;
    const float BACKGROUND_SPEED_LEVEL_0 = 0.1f;
    const float GROUND_SPEED_LEVEL_0 = 1.75f;

    const float FRONT_ROCKS_SPEED_LEVEL_1 = 1.5f;
    const float BACK_ROCKS_SPEED_LEVEL_1 = 1.5f;
    const float MOUNTAINS_SPEED_LEVEL_1 = 0.4f;
    const float BACKGROUND_SPEED_LEVEL_1 = 0.1f;
    const float GROUND_SPEED_LEVEL_1 = 3f;


    private void OnEnable()
    {
        GameManager.OnDifficultyUp += HandleDifficultyUp;
    }

    private void OnDisable()
    {
        GameManager.OnDifficultyUp -= HandleDifficultyUp;
    }

    void HandleDifficultyUp(int level) // TODO: create script for each bg element that inherits translate
    {
        switch (level)
        {
            case 0:
                SetPrefabSpeeds(FRONT_ROCKS_TAG, FRONT_ROCKS_SPEED_LEVEL_0);
                SetPrefabSpeeds(BACK_ROCKS_TAG, BACK_ROCKS_SPEED_LEVEL_0);
                SetPrefabSpeeds(MOUNTAINS_TAG, MOUNTAINS_SPEED_LEVEL_0);
                SetPrefabSpeeds(BACKGROUND_TAG, BACKGROUND_SPEED_LEVEL_0);
                SetPrefabSpeeds(GROUND_TAG, GROUND_SPEED_LEVEL_0);
                break;
            case 1:
                SetPrefabSpeeds(FRONT_ROCKS_TAG, FRONT_ROCKS_SPEED_LEVEL_1);
                SetPrefabSpeeds(BACK_ROCKS_TAG, BACK_ROCKS_SPEED_LEVEL_1);
                SetPrefabSpeeds(MOUNTAINS_TAG, MOUNTAINS_SPEED_LEVEL_1);
                SetPrefabSpeeds(BACKGROUND_TAG, BACKGROUND_SPEED_LEVEL_1);
                SetPrefabSpeeds(GROUND_TAG, GROUND_SPEED_LEVEL_1);
                break;
            default:
                break;
        }
    }

    void SetPrefabSpeeds(string tag, float speed)
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject prefab in prefabs)
        {
            prefab.GetComponent<Translate>().speed = speed;
        }
    }
}

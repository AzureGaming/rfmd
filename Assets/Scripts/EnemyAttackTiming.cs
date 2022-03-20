using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTiming : MonoBehaviour
{
    DodgeTimingManager timingManager;

    private void Awake()
    {
        timingManager = FindObjectOfType<DodgeTimingManager>();
    }

    public void StartHighAttackTimer()
    {
        StartCoroutine(timingManager.StartTimer(0.5f));
    }

    public void Level1Dodge()
    {
        timingManager.level = DodgeTimingManager.DodgeLevel.LEVEL_1;
    }

    public void Level2Dodge()
    {
        timingManager.level = DodgeTimingManager.DodgeLevel.LEVEL_2;
    }
}

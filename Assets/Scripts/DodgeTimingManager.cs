using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeTimingManager : MonoBehaviour
{
    public DodgeLevel level;
    public enum DodgeLevel
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3
    }
    bool playerDodged;

    public IEnumerator StartTimer(float endTime)
    {
        Debug.Log("Timer started");
        float timeElapsed = 0f;
        playerDodged = false;

        yield return new WaitUntil(() =>
        {
            bool isTimerDone = endTime >= timeElapsed;
            timeElapsed += Time.deltaTime;

            return isTimerDone || playerDodged;
        });

        yield return GetDodgeLevel(timeElapsed, endTime);
    }

    public void DodgeInputted()
    {
        playerDodged = true;
    }

    DodgeLevel GetDodgeLevel(float result, float target)
    {
        Debug.Log($"result {result} , target {target}");
        level = DodgeLevel.LEVEL_1;
        return DodgeLevel.LEVEL_1;
    }
}

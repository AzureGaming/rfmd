using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    List<Achievement> achievements;

    private void Start()
    {
        if (achievements == null)
        {
            InitializeAchievements();
        }
    }

    private void Update()
    {
        CheckAchievementCompletion();
    }

    void InitializeAchievements()
    {
        Achievement[] achievementsToInit = GetComponents<Achievement>();
        achievements = new List<Achievement>();
        foreach (Achievement achievement in achievementsToInit)
        {
            achievements.Add(achievement);
        }
    }

    void CheckAchievementCompletion()
    {
        if (achievements == null)
        {
            return;
        }

        foreach (Achievement achievement in achievements)
        {
            achievement.UpdateCompletion();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator enemyAnimator;
    public Animator playerAnimator;
    public Image overlay;

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DeleteAchievementProgress()
    {
        FindObjectOfType<PlayerStatsTracker>().DeleteAllPrefs();
    }

    IEnumerator PlayRoutine()
    {
        enemyAnimator.SetTrigger("Awake");
        playerAnimator.SetTrigger("Awake");
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene("Game");
    }

    IEnumerator FadeOut()
    {
        float totalTime = 1f;
        float timeElapsed = 0f;
        Color startColour = overlay.color;
        Color newColour = startColour;
        newColour.a = 1f;
        while (timeElapsed <= totalTime)
        {
            Color newCol = Color.Lerp(startColour, newColour, (timeElapsed / totalTime));
            overlay.color = newCol;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}

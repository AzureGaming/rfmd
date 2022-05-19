using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text totalCurrencyText;
    public Animator enemyAnimator;
    public Animator playerAnimator;
    public Image overlay;

    private void OnEnable()
    {
        SetTotalCurrencyText();
    }

    void SetTotalCurrencyText()
    {
        totalCurrencyText.text = $"Currency: {FindObjectOfType<PlayerStatsTracker>().GetTotalCurrency()}E";
    }

    public void Play()
    {
        FindObjectOfType<GameManager>()?.StartRun();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DeleteAchievementProgress()
    {
        FindObjectOfType<PlayerStatsTracker>().DeleteAllPrefs();
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

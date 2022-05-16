using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text enemiesText;
    [SerializeField] TMP_Text currencyText;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        SetEnemiesKilled();
        SetCurrencyCollected();
    }

    public void Continue()
    {
        SceneManager.LoadScene("Sanctuary");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    void SetEnemiesKilled()
    {
        enemiesText.text = $"{gameManager.enemiesKilled} DEMONS";
    }

    void SetCurrencyCollected()
    {
        currencyText.text = $"{gameManager.runCurrency}E";
    }
}

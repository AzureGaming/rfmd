using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text totalCurrencyText;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        animator.SetTrigger("Home_Exit");
    }

    public void StartRun()
    {
        FindObjectOfType<GameManager>()?.StartRun();
    }
}

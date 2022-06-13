using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [SerializeField] TMP_Text currencyText;

    private void Update()
    {
        SetCurrencyText();
    }

    void SetCurrencyText()
    {
        currencyText.text = $"{FindObjectOfType<PlayerStatsTracker>().GetTotalCurrency()}";
    }
}

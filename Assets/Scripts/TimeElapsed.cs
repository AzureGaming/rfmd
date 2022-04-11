using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeElapsed : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetText(int minutes, int seconds)
    {
        text.text = $"Time Elapsed: {minutes}.{seconds}";
    }
}

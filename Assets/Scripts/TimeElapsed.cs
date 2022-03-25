using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeElapsed : MonoBehaviour
{
    TMP_Text text;

    int time;
    int seconds;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        time = 0;
        seconds = 0;
    }

    private void Update()
    {
        time = (int)Time.time / 60;
        seconds = (int)Time.time % 60;

        SetText(time, seconds);
    }

    void SetText(int minutes, int seconds)
    {
        text.text = $"Time Elapsed: {minutes}.{seconds}";
    }
}

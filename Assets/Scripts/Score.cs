using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TMP_Text text;
    ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Score: " + scoreManager.score;
    }
}

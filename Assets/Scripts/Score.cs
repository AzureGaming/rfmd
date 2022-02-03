using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TMP_Text text;
    GameManager gameManager;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        text.text = $"Score: {gameManager.score}";
    }
}

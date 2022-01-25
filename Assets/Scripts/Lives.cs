using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lives : MonoBehaviour
{
    TMP_Text text;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Lives: " + gameManager.lives;
    }
}

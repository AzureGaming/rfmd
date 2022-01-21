using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; }

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        score = 0;
        StartCoroutine(AddScore());
    }

    IEnumerator AddScore()
    {
        while (gameManager.isPlayerAlive)
        {
            score++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

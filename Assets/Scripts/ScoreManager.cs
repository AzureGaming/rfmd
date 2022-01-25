using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; }

    GameManager gameManager;
    Coroutine addScoreRoutine;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        score = 0;
        addScoreRoutine = StartCoroutine(AddScore());
    }

    public void Stop()
    {
        StopCoroutine(addScoreRoutine);
    }

    public void AddBonus(int val)
    {
        score += val;
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

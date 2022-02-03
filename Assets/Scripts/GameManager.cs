using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlayerAlive;

    [SerializeField] int lives;
    [SerializeField] int dodgePoints = 50;
    [SerializeField] int scoreIncrement;
    [SerializeField] int level2Threshold = 500;
    [SerializeField] int level3Threshold = 1000;
    [SerializeField] GameObject loseScreen;

    public int level = 1;
    public int score;

    Coroutine gameRoutine;

    private void Start()
    {
        FindObjectOfType<Lives>().Init(lives);
        gameRoutine = StartCoroutine(GameRoutine());
    }

    public void PlayerDied()
    {
        FindObjectOfType<Enemy>().Stop();
        loseScreen.SetActive(true);
        StopCoroutine(gameRoutine);
    }

    public void PlayerHit()
    {
        lives--;
        FindObjectOfType<Lives>().SetHearts(lives);
    }

    public void PlayerDodged()
    {
        score += dodgePoints;
    }

    public int GetLives()
    {
        return lives;
    }

    IEnumerator GameRoutine()
    {
        while (isPlayerAlive)
        {
            score+= scoreIncrement;
            if (score >= level2Threshold && level == 1 || score >= level3Threshold && level == 2)
            {
                level++;
                Debug.Log($"Reached level {level}!");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

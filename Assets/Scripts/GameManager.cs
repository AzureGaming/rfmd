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

    Coroutine scoreRoutine;

    private void Start()
    {
        FindObjectOfType<Lives>().Init(lives);
        scoreRoutine = StartCoroutine(ScoreRoutine());
        StartCoroutine(LevelUpRoutine());
    }

    public void PlayerDied()
    {
        FindObjectOfType<Enemy>().Stop();
        loseScreen.SetActive(true);
        StopCoroutine(scoreRoutine);
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

    IEnumerator ScoreRoutine()
    {
        while (isPlayerAlive)
        {
            score += scoreIncrement;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator LevelUpRoutine()
    {
        float timeElapsed = 0f;
        while (isPlayerAlive)
        {
            timeElapsed += Time.deltaTime;
            if (level == 1 && timeElapsed >= 15f || level == 2 && timeElapsed >= 30f || level == 3 && timeElapsed >= 45f)
            {
                level++;
                Enemy.OnLevelUp(level);
                Debug.Log($"Reached level {level}!");
            }



            yield return null;
        }
    }
}

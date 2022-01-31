using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isPlayerAlive;

    [SerializeField] int lives;
    [SerializeField] int dodgePoints = 50;
    [SerializeField] GameObject loseScreen;

    private void Start()
    {
        FindObjectOfType<Lives>().Init(lives);
    }

    public void PlayerDied()
    {
        FindObjectOfType<ScoreManager>().Stop();
        FindObjectOfType<Enemy>().Stop();
        loseScreen.SetActive(true);
    }

    public void PlayerHit()
    {
        lives--;
        FindObjectOfType<Lives>().SetHearts(lives);
    }

    public void PlayerDodged()
    {
        FindObjectOfType<ScoreManager>().AddBonus(dodgePoints);
    }

    public int GetLives()
    {
        return lives;
    }

    public int GetScore()
    {
        return FindObjectOfType<ScoreManager>().score;
    }
}

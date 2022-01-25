using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public bool isPlayerAlive;
    public int lives;

    [SerializeField]
    int dodgePoints = 50;

    public void PlayerDied()
    {
        FindObjectOfType<ScoreManager>().Stop();
        FindObjectOfType<Enemy>().Stop();
    }

    public void PlayerDodged()
    {
        FindObjectOfType<ScoreManager>().AddBonus(dodgePoints);
    }
}

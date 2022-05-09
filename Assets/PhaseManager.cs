using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject bossHealthBar;

    GameManager gameManager;
    bool bossPhased;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        SetEnemyPhase();
    }

    private void Update()
    {
        CheckPhase(gameManager.minutes, gameManager.seconds);
    }

    void CheckPhase(int minutes, int seconds)
    {
        if (minutes == 1 && seconds == 0 && !bossPhased)
        {
            bossPhased = true;
            SetBossPhase();
        }
    }

    void SetEnemyPhase()
    {
        GameObject.FindGameObjectWithTag("BossHealthBar")?.SetActive(false);
        FindObjectOfType<EnemySpawner>()?.StartSpawning();
    }

    void SetBossPhase()
    {
        FindObjectOfType<EnemySpawner>()?.StopSpawning();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Die();
        }
    }
}

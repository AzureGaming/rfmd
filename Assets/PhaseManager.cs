using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform playerEnemyPos;
    [SerializeField] Transform playerBossPos;
    [SerializeField] Transform bossPos;
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
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.transform.position = playerEnemyPos.position;
        }
        else
        {
            Instantiate(playerPrefab, playerEnemyPos, true);
        }

        FindObjectOfType<EnemySpawner>()?.StartSpawning();
        bossHealthBar.SetActive(false);
    }

    void SetBossPhase()
    {
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.transform.position = playerBossPos.position;
        }
        else
        {
            Instantiate(playerPrefab, playerBossPos, true);
        }

        FindObjectOfType<EnemySpawner>()?.StopSpawning();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Die();
        }
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab, bossPos, true);
    }
}

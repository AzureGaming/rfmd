using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject bossHealthBar;

    GameManager gameManager;
    bool bossPhased;
    bool isBossAlive;

    private void OnEnable()
    {
        Boss.OnDeath += SetBossDeath;
    }

    private void OnDisable()
    {
        Boss.OnDeath -= SetBossDeath;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        CheckPhase(gameManager.minutes, gameManager.seconds);
    }

    public void InitPhases()
    {
        if (FindObjectOfType<Player>() == null)
        {
            Instantiate(playerPrefab, playerPos, true);
        }
        SetEnemyPhase();
    }

    void SetEnemyPhase()
    {
        bossHealthBar.SetActive(false);
        FindObjectOfType<EnemySpawner>()?.StartSpawning();
    }

    void CheckPhase(int minutes, int seconds)
    {
        if (minutes == 1 && seconds == 0 && !bossPhased)
        {
            bossPhased = true;
            SetBossPhase();
        }
        if (bossPhased && !isBossAlive)
        {
            bossPhased = false;
        }
    }

    void SetBossPhase()
    {
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        FindObjectOfType<EnemySpawner>()?.StopSpawning();
        foreach (Enemy enemy in enemies)
        {
            enemyManager.Remove(enemy);
            Destroy(enemy.gameObject);
        }
        bossHealthBar.SetActive(true);
        Instantiate(bossPrefab);
        isBossAlive = true;
    }

    void SetBossDeath()
    {
        isBossAlive = false;
        Destroy(FindObjectOfType<Player>().gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;
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
        if (minutes == 0 && seconds == 3 && !bossPhased)
        {
            bossPhased = true;
            SetBossPhase();
        }
        if (bossPhased && !isBossAlive)
        {
            bossPhased = false;
        }
    }

    void SetEnemyPhase()
    {
        bossHealthBar.SetActive(false);
        FindObjectOfType<EnemySpawner>()?.StartSpawning();
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

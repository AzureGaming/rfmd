using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void Spawned(Enemy enemy);
    public static Spawned OnSpawned;

    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject bossSpawnPos;
    GameManager gameManager;
    Coroutine spawnRoutine;
    Transform spawnPos;
    List<GameObject> enemyRefs;
    GameObject enemyToSpawn;

    const float DELAY_MIN_LEVEL_0 = 3f;
    const float DELAY_MAX_LEVEL_0 = 5f;
    const float DELAY_MIN_LEVEL_1 = 4f;
    const float DELAY_MAX_LEVEL_1 = 5f;

    private void Awake()
    {
        enemyRefs = new List<GameObject>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        spawnRoutine = StartCoroutine(HandleSpawning());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnRoutine);
    }

    public void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPos.transform);
    }
    IEnumerator HandleSpawning()
    {
        for (; ; )
        {
            if (enemyRefs.Count < spawnPositions.Length)
            {
                yield return StartCoroutine(Delay());
                ChooseEnemyToSpawn();
                SetSpawnPosition();
                Spawn();
            }
            yield return null;
        }
    }

    IEnumerator Delay()
    {
        int level = gameManager.level;

        //if (level == 0)
        //{
        //    yield return new WaitForSeconds(Random.Range(DELAY_MIN_LEVEL_0, DELAY_MAX_LEVEL_0));
        //}
        //else
        //{
        //    yield return new WaitForSeconds(Random.Range(DELAY_MIN_LEVEL_1, DELAY_MAX_LEVEL_1));
        //}
        yield return new WaitForSeconds(1f);
    }

    void Spawn()
    {
        if (spawnPos)
        {
            Enemy enemy = Instantiate(enemyToSpawn, spawnPos).GetComponent<Enemy>();
            OnSpawned?.Invoke(enemy);
        }
    }

    void ChooseEnemyToSpawn()
    {
        enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    void SetSpawnPosition()
    {
        List<Transform> validSpawnPositions = new List<Transform>();
        foreach (Transform position in spawnPositions)
        {
            if (position.childCount == 0)
            {
                validSpawnPositions.Add(position);
            }
        }

        if (validSpawnPositions.Count == 0)
        {
            spawnPos = null;
        }
        else
        {
            spawnPos = validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];

        }
    }
}

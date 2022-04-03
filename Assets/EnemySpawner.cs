using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject bossSpawnPos;
    Coroutine spawnRoutine;
    Transform spawnPos;
    List<GameObject> enemyRefs;

    private void Awake()
    {
        enemyRefs = new List<GameObject>();
    }

    private void Start()
    {
        spawnRoutine = StartCoroutine(HandleSpawning());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnRoutine);
    }

    IEnumerator HandleSpawning()
    {
        for (; ; )
        {
            if (enemyRefs.Count < spawnPositions.Length)
            {
                yield return new WaitForSeconds(1f); // spawn timer?
                SetSpawnPosition();
                Spawn();
            }
            yield return null;
        }
    }

    void Spawn()
    {
        if (spawnPos)
        {
            Instantiate(enemyPrefab, spawnPos);
        }
    }

    public void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPos.transform);
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

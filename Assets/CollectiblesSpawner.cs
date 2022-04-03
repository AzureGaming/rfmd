using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;
    Spawner[] spawners;
    Spawner spawner;
    float spawnTimer;
    float spawnTimerMax = 1f;

    private void Awake()
    {
        spawners = GetComponentsInChildren<Spawner>();
    }

    private void Update()
    {
        HandleSpawning();
    }

    void HandleSpawning()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnTimerMax;
            GetRandomSpawner();
            Spawn();
        }
    }

    void GetRandomSpawner()
    {
        spawner = spawners[Random.Range(0, 3)];
    }

    void Spawn()
    {
        spawner.Spawn(prefabs[0]);
    }
}

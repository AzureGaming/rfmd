using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemyRefs { get; private set; }

    private void Awake()
    {
        enemyRefs = new List<Enemy>();
    }

    private void OnEnable()
    {
        EnemySpawner.OnSpawned += Add;
        Enemy1.OnDeath += Remove;
        Enemy2.OnDeath += Remove;
    }

    private void OnDisable()
    {
        EnemySpawner.OnSpawned -= Add;
        Enemy1.OnDeath -= Remove;
        Enemy2.OnDeath -= Remove;
    }

    private void Start()
    {
        // if enemies were not added by spawner
        Enemy[] initialEnemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in initialEnemies)
        {
            enemyRefs.Add(enemy);
        }
    }

    public void Add(Enemy enemy)
    {
        enemyRefs.Add(enemy);
    }

    public void Remove(Enemy enemy)
    {
        enemyRefs.Remove(enemy);
    }
}

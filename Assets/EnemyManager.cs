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
        Enemy.OnSpawned += Add;
        Enemy.OnDeath += Remove;
    }

    private void OnDisable()
    {
        Enemy.OnSpawned -= Add;
        Enemy.OnDeath -= Remove;
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

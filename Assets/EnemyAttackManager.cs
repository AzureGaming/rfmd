using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    Enemy attackingEnemy;
    GameManager gameManager;
    List<Enemy> enemyRefs;
    Coroutine routine;

    const float ATTACK_DELAY_MAX_LEVEL_0 = 3.5f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;
    bool isActionDone = true; // flag set when player dodges or enemy dies 

    private void OnEnable()
    {
        GameManager.OnDamageEnemy += ActiveEnemyTakeDamage;
        EnemySpawner.OnSpawned += AddEnemy;
        GameManager.OnActionDone += CompleteAction;
    }

    private void OnDisable()
    {
        GameManager.OnDamageEnemy -= ActiveEnemyTakeDamage;
        EnemySpawner.OnSpawned -= AddEnemy;
        GameManager.OnActionDone -= CompleteAction;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyRefs = new List<Enemy>();
    }

    private void Start()
    {
        StartCoroutine(Routine());
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyRefs.Remove(enemy);
    }

    public void ActiveEnemyTakeDamage(int damage)
    {
        if (attackingEnemy != null)
        {
            attackingEnemy.TakeDamage(damage);
        }
    }

    IEnumerator Routine()
    {
        for (; ; )
        {
            if (enemyRefs.Count > 0 && isActionDone)
            {
                // TODO: fix first enemy attacking instantly
                isActionDone = false;
                EnemyAttack();
                yield return new WaitWhile(() => !isActionDone);
                yield return new WaitForSeconds(GetAttackDelay());
            }
            yield return null;
        }
    }

    void EnemyAttack()
    {
        Enemy enemyToAttack = enemyRefs[Random.Range(0, enemyRefs.Count)];
        attackingEnemy = enemyToAttack;
        StartCoroutine(enemyToAttack.Attack());
    }

    float GetAttackDelay()
    {
        int level = gameManager.level;
        if (level == 0)
        {
            return Random.Range(ATTACK_DELAY_MIN_LEVEL_0, ATTACK_DELAY_MAX_LEVEL_0);
        }
        else
        {
            return Random.Range(ATTACK_DELAY_MIN_LEVEL_1, ATTACK_DELAY_MAX_LEVEL_1);
        }
    }

    void AddEnemy(Enemy enemy)
    {
        enemyRefs.Add(enemy);
    }

    void CompleteAction()
    {
        isActionDone = true;
    }
}

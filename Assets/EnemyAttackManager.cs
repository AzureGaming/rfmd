using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    Enemy attackingEnemy;
    GameManager gameManager;

    const float ATTACK_DELAY_MAX_LEVEL_0 = 4f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;

    private void OnEnable()
    {
        GameManager.OnDamageEnemy += ActiveEnemyTakeDamage;
    }

    private void OnDisable()
    {
        GameManager.OnDamageEnemy -= ActiveEnemyTakeDamage;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(Routine());
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
            GameObject[] enemyRefs = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemyRefs.Length > 0)
            {
                attackingEnemy = enemyRefs[Random.Range(0, enemyRefs.Length - 1)].GetComponent<Enemy>();
                yield return StartCoroutine(attackingEnemy.Attack());
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    float GetAttackDelay()
    {
        int level = gameManager.level;
        if (level == 0)
        {
            return Random.Range(ATTACK_DELAY_MIN_LEVEL_0, ATTACK_DELAY_MAX_LEVEL_0);
        } else
        {
            return Random.Range(ATTACK_DELAY_MIN_LEVEL_1, ATTACK_DELAY_MAX_LEVEL_1);
        }
    }
}

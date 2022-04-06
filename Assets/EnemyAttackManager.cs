using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    Enemy chosen;
    GameManager gameManager;
    EnemyManager enemyManager;
    bool isEnemyAttacking = false;
    float attackTimer;

    const float ATTACK_DELAY_MAX_LEVEL_0 = 3.5f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;

    private void OnEnable()
    {
        GameManager.OnDamageEnemy += EnemyDamaged;
        GameManager.OnDamagePlayer += PlayerDamaged;
    }

    private void OnDisable()
    {
        GameManager.OnDamageEnemy -= EnemyDamaged;
        GameManager.OnDamagePlayer -= PlayerDamaged;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        if (!chosen)
        {
            ChooseRandomEnemy();
            SetAttackTimer();
        }

        if (chosen && attackTimer <= 0f)
        {
            isEnemyAttacking = true;
            chosen.Attack();
        }

        attackTimer -= Time.deltaTime;
    }

    void ChooseRandomEnemy()
    {
        List<Enemy> availableEnemies = enemyManager.enemyRefs;
        if (availableEnemies.Count > 0)
        {
            chosen = availableEnemies[Random.Range(0, availableEnemies.Count)];
        }
    }

    void EnemyDamaged(int damage)
    {
        if (isEnemyAttacking)
        {
            chosen?.TakeDamage(damage);
        }
        isEnemyAttacking = false;
        chosen = null;
    }

    void PlayerDamaged()
    {
        isEnemyAttacking = false;
        chosen = null;
    }

    void SetAttackTimer()
    {
        attackTimer = GetAttackDelay();
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
}

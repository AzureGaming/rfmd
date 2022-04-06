using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    GameManager gameManager;
    EnemyManager enemyManager;
    List<Enemy> attackingEnemies;
    Enemy chosen;
    float attackTimer;

    const float ATTACK_DELAY_MAX_LEVEL_0 = 3.5f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;
    const float ATTACK_DELAY_LEVEL_2 = 2f;
    const float ATTACK_DELAY_LEVEL_3 = 1f;
    const float ATTACK_DELAY_LEVEL_4 = 0.5f;

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

        if (IsValidAttack())
        {
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

    void EnemyDamaged(int _)
    {
        CompleteAttack();
    }

    void PlayerDamaged()
    {
        CompleteAttack();
    }

    void CompleteAttack()
    {
        Debug.Log("Enemy finsiehd attack");
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
        if (level == 1)
        {
            return Random.Range(ATTACK_DELAY_MIN_LEVEL_1, ATTACK_DELAY_MAX_LEVEL_1);
        }
        if (level == 2)
        {
            return ATTACK_DELAY_LEVEL_2;
        }
        if (level == 3)
        {
            return ATTACK_DELAY_LEVEL_3;
        }
        return ATTACK_DELAY_LEVEL_4;
    }

    bool IsValidAttack()
    {
        float timerBuffer = 0f + 1f;
        return chosen && (attackTimer <= timerBuffer);
    }
}

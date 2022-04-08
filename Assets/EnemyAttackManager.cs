using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    EnemyManager enemyManager;
    Dictionary<Enemy, float> cooldownMap;
    List<Enemy> enemyList;

    const float MAX_PLAYER_REACTION = 1f;
    const float ATTACK_DELAY_MAX_LEVEL_0 = 3.5f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;
    const float ATTACK_DELAY_LEVEL_2 = 2f;
    const float ATTACK_DELAY_LEVEL_3 = 1f;
    const float ATTACK_DELAY_LEVEL_4 = 0.5f;

    private void OnEnable()
    {
        Enemy.OnFinishAttackAnimation += SetEnemyCooldown;
    }

    private void OnDisable()
    {
        Enemy.OnFinishAttackAnimation -= SetEnemyCooldown;
    }

    private void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        cooldownMap = new Dictionary<Enemy, float>();
        enemyList = new List<Enemy>();
    }

    private void Update()
    {
        UpdateTrackedEnemies();
        DecrementCooldowns();
        ExecuteAttacks();
    }

    void UpdateTrackedEnemies()
    {
        List<Enemy> availableEnemies = enemyManager.enemyRefs;
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy enemy in enemyList)
        {
            if (!availableEnemies.Contains(enemy))
            {
                enemiesToRemove.Add(enemy);
            }
        }
        foreach (Enemy enemy in enemiesToRemove)
        {
            enemyList.Remove(enemy);
            cooldownMap.Remove(enemy);
        }

        foreach (Enemy enemy in availableEnemies)
        {
            if (!enemyList.Contains(enemy))
            {
                enemyList.Add(enemy);
            }
            if (!cooldownMap.ContainsKey(enemy))
            {
                cooldownMap[enemy] = GetCooldown(enemy);
            }
        }
    }

    void ExecuteAttacks()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (cooldownMap[enemy] <= 0f)
            {
                enemy.Attack();
            }
        }
    }

    void SetEnemyCooldown(Enemy enemy)
    {
        if (cooldownMap.Keys.Contains(enemy))
        {
            cooldownMap[enemy] = GetCooldown(enemy);
        }
    }

    float GetCooldown(Enemy enemy)
    {
        float attackAnimationSpeed = enemy.animationLength;
        float playerLockout = 0.5f;
        float minCooldown = attackAnimationSpeed + playerLockout;
        foreach (KeyValuePair<Enemy, float> cooldownkp in cooldownMap)
        {
            if (enemy.isAttacking)
            {
                continue;
            }

            if (!IsValidCooldown(minCooldown, cooldownkp.Value))
            {
                minCooldown = cooldownkp.Value + MAX_PLAYER_REACTION;
            }
        }

        float cooldown = Random.Range(minCooldown, (float)(minCooldown * 2));

        enemy.GetComponentInChildren<CooldownText>()?.SetText(cooldown);
        return cooldown;
    }

    bool IsValidCooldown(float cooldown, float comparisonCooldown)
    {
        return Mathf.Abs(cooldown - comparisonCooldown) > MAX_PLAYER_REACTION;
    }

    void DecrementCooldowns()
    {
        foreach (Enemy enemy in enemyList)
        {
            if (cooldownMap[enemy] > 0f)
            {
                cooldownMap[enemy] -= Time.deltaTime;
                if (cooldownMap[enemy] <= 0f)
                {
                    cooldownMap[enemy] = 0f;
                }
                enemy.GetComponentInChildren<CooldownText>()?.SetText(cooldownMap[enemy]);
            }
        }
    }
}

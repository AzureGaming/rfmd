using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    EnemyManager enemyManager;
    Dictionary<Enemy, float> cooldownMap;
    List<Enemy> enemyList;

    const float MAX_PLAYER_REACTION = 1.5f;
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
        float minCooldown = attackAnimationSpeed + MAX_PLAYER_REACTION;
        List<List<float>> cooldownRanges = new List<List<float>>();

        List<float> cooldowns = cooldownMap.Values.ToList();
        cooldowns = cooldowns.OrderBy((float cd) => cd).ToList();
        for (int i = 0; i < cooldowns.Count; i++)
        {
            float cooldown = cooldowns[i];
            float prevCooldown;
            if (i == 0)
            {
                prevCooldown = 0f;
            }
            else
            {
                prevCooldown = cooldowns[i - 1];
            }
            float nextCooldown;
            if (i == cooldowns.Count - 1)
            {
                nextCooldown = cooldown + (2 * MAX_PLAYER_REACTION);
            }
            else
            {
                nextCooldown = cooldowns[i + 1];
            }
            float max = cooldown - MAX_PLAYER_REACTION;
            float min = prevCooldown + MAX_PLAYER_REACTION;
            if (!IsCooldownRangeReactable(min, max))
            {
                continue;
            }
            cooldownRanges.Add(new List<float>() { min, max });
        }

        float chosenCooldown;
        if (cooldowns.Count < 1)
        {
            // this is the only enemy on screen
            chosenCooldown = Random.Range(minCooldown, (minCooldown * 2));
        } else if (cooldownRanges.Count < 1)
        {
            // multiple enemies on screen but theres no space between them
            float min2 = cooldowns.Last() + MAX_PLAYER_REACTION;
            float max2 = min2 + MAX_PLAYER_REACTION;
            chosenCooldown = Random.Range(min2, max2);
        }
        else
        {
            List<float> range = cooldownRanges[Random.Range(0, cooldownRanges.Count)];
            chosenCooldown = Random.Range(range[0], range[1]);
        }
        enemy.GetComponentInChildren<CooldownText>()?.SetText(chosenCooldown);
        return chosenCooldown;
    }

    bool IsCooldownRangeReactable(float min, float max)
    {
        return (max - min) >= MAX_PLAYER_REACTION;
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

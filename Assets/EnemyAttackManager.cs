using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    EnemyManager enemyManager;
    GameManager gameManager;
    Dictionary<Enemy, float> cooldownMap;
    List<Enemy> enemyList;

    const float PLAYER_REACTION_LEVEL_1 = 1.5f;
    const float PLAYER_REACTION_LEVEL_2 = 1.25f;
    const float PLAYER_REACTION_LEVEL_3 = 1f;

    const float ATTACK_DELAY_MAX_LEVEL_1 = 3.5f;
    const float ATTACK_DELAY_MIN_LEVEL_1 = 3f;
    const float ATTACK_DELAY_MAX_LEVEL_2 = 3f;
    const float ATTACK_DELAY_MIN_LEVEL_2 = 2f;
    const float ATTACK_DELAY_LEVEL_3 = 2f;
    const float ATTACK_DELAY_LEVEL_4 = 1f;
    const float ATTACK_DELAY_LEVEL_5 = 0.5f;

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
        gameManager = FindObjectOfType<GameManager>();
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
        float reactionTime = GetReactionTime();
        float attackAnimationSpeed = enemy.animationLength;
        float minCooldown = attackAnimationSpeed + reactionTime;
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
                nextCooldown = cooldown + (1.5f * reactionTime);
            }
            else
            {
                nextCooldown = cooldowns[i + 1];
            }
            float max = cooldown - reactionTime;
            float min = prevCooldown + reactionTime;
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
            chosenCooldown = Random.Range(minCooldown, (minCooldown + reactionTime));
        }
        else if (cooldownRanges.Count < 1)
        {
            // multiple enemies on screen but theres no space between them
            float min2 = cooldowns.Last() + reactionTime;
            float max2 = min2 + reactionTime;
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
        return (max - min) >= GetReactionTime();
    }

    float GetReactionTime()
    {
        int level = gameManager.level;
        if (level == 1)
        {
            return PLAYER_REACTION_LEVEL_1;
        }
        if (level == 2)
        {
            return PLAYER_REACTION_LEVEL_2;
        }
        //if (level == 2)
        //{
        return PLAYER_REACTION_LEVEL_3;
        //}
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

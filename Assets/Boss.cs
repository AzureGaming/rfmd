using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public delegate void Impact(AttackType attackType);
    public static Impact OnImpact;
    public delegate void Death();
    public static Death OnDeath;
    public delegate void Spawn();
    public static Spawn OnSpawn;

    public float DAMAGE_REDUCTION { get; private set; } = 0.5f;

    const int MAX_HEALTH = 100;
    protected bool isAttacking;
    protected AttackType attackType;
    protected HealthBar healthBar;

    protected virtual void Awake()
    {
        healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);

        OnSpawn?.Invoke();
        StartCoroutine(BossRoutine());
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        if (health <= 0)
        {
            Die();
        }
    }

    protected void SetHealth(int val)
    {
        health = val;
        healthBar.SetHealth(health);
    }

    protected virtual IEnumerator BossRoutine() { yield break; }

    protected virtual void Die() { }
}

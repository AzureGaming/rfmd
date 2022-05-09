using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Enemy
{
    public delegate void Death();
    public static Death OnDeath;
    public delegate void Spawn();
    public static Spawn OnSpawn;
    public delegate void Damaged(int health, int maxHealth);
    public static Damaged OnDamaged;

    public float DAMAGE_REDUCTION { get; private set; } = 0.1f;

    protected virtual int MAX_HEALTH { get; set; }
    protected HealthBar healthBar;

    protected virtual void Awake()
    {
        healthBar = GameObject.FindGameObjectWithTag("BossHealthBar").GetComponent<HealthBar>();
    }

    protected virtual void Start()
    {
        healthBar.gameObject.SetActive(true);
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);

        OnSpawn?.Invoke();
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        OnDamaged?.Invoke(health, MAX_HEALTH);
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

    protected virtual void Die() {
        healthBar.gameObject.SetActive(false);
    }
}

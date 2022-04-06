using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField] GameObject bloodSplatPrefab;
    HealthBar healthBar;
    Animator anim;
    new Enemy1Audio audio;

    const int MAX_HEALTH = 10;
    const AttackType ATTACK_TYPE = AttackType.Low;
    bool isAttacking;
    bool waitingForHitResponse;

    private void OnEnable()
    {
        Player.OnHit += HandleHitSuccess;
    }

    private void OnDisable()
    {
        Player.OnHit -= HandleHitSuccess;
    }

    private void Awake()
    {
        healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
        anim = GetComponent<Animator>();
        audio = GetComponent<Enemy1Audio>();
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
        OnSpawned?.Invoke(this);
    }

    private void Update()
    {
        if (isAttacking)
        {
            isAttacking = false;
            anim.SetTrigger("Attack");
            waitingForHitResponse = true;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public override void Attack()
    {
        isAttacking = true;
    }

    public void CheckHit()
    {
        OnImpact?.Invoke(ATTACK_TYPE, this);
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(health - damage);
    }

    void HandleHitSuccess()
    {
        if (waitingForHitResponse)
        {
            audio.PlayImpact();
            waitingForHitResponse = false;
        }
    }

    void SetHealth(int val)
    {
        health = val;
        healthBar.SetHealth(health);
    }

    void Die()
    {
        OnDeath.Invoke(this);
        audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }
}

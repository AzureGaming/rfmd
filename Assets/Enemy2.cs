using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    public delegate void Impact(AttackType attackType);
    public static Impact OnImpact;
    public delegate void Death();
    public static Death OnDeath;

    [SerializeField] GameObject bloodSplatPrefab;
    HealthBar healthBar;
    Animator anim;
    new Enemy1Audio audio;

    const int MAX_HEALTH = 10;
    const float DESTROY_DELAY = 0.5f;
    const AttackType ATTACK_TYPE = AttackType.Low;
    bool isAttacking;

    private void OnEnable()
    {
        //Player.OnHit += audio.PlayImpact;
    }

    private void OnDisable()
    {
        //Player.OnHit -= audio.PlayImpact;
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
    }

    public void CheckHit()
    {
        isAttacking = false;
    }

    public override IEnumerator Attack()
    {
        // Note: this coroutine may still be running when gameobj is destroyed due to death.
        isAttacking = true;
        anim.SetTrigger("Attack");
        audio.PlayTelegraph();
        yield return new WaitUntil(() => !isAttacking);
        OnImpact?.Invoke(ATTACK_TYPE);
        audio.PlayAttack();
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        if (health <= 0)
        {
            Die();
        }
    }

    void SetHealth(int val)
    {
        health = val;
        healthBar.SetHealth(health);
    }

    void Die()
    {
        OnDeath.Invoke();
        audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        FindObjectOfType<EnemyAttackManager>().RemoveEnemy(this);
        GameManager.OnActionDone?.Invoke();
        Destroy(gameObject);
    }
}

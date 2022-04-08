using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Boss
{
    enum AttackPatterns
    {
        Basic1,
        Basic2
    }

    [SerializeField] GameObject bloodSplatPrefab;
    Animator anim;
    SpriteRenderer spriteR;
    new Boss1Audio audio;
    AttackPatterns attackPattern;

    new protected const int MAX_HEALTH = 200;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        audio = GetComponent<Boss1Audio>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void CheckHitLow()
    {
        OnImpact?.Invoke(AttackType.Low, this);
    }

    public void CheckHitHigh()
    {
        OnImpact?.Invoke(AttackType.High, this);
    }

    public void CompleteAttack()
    {
        isAttacking = false;
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
    }

    private void Update()
    {
        if (health > 0)
        {
            ChooseAttack();
            ExecuteAttack();
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void ChooseAttack()
    {
        int attackType = Random.Range(0, 2);
        if (attackType == 0)
        {
            attackPattern = AttackPatterns.Basic1;
        }
        if (attackType == 1)
        {
            attackPattern = AttackPatterns.Basic2;
        }
    }

    void ExecuteAttack()
    {
        if (attackPattern == AttackPatterns.Basic1)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetTrigger("Attack1");
            }
        }
        if (attackPattern == AttackPatterns.Basic2)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetTrigger("Attack2");
            }
        }
    }

    protected override void Die()
    {
        OnDeath?.Invoke();
        audio.PlayDeath();
        spriteR.color = Color.clear;
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }
}

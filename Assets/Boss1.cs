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
    float attackTimer;
    CooldownText cdText;

    protected override int MAX_HEALTH { get { return 200; } }

    private void OnEnable()
    {
        Player.OnHit += HandleHitSuccess;
    }

    private void OnDisable()
    {
        Player.OnHit -= HandleHitSuccess;
    }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        audio = GetComponent<Boss1Audio>();
        spriteR = GetComponent<SpriteRenderer>();
        cdText = GetComponentInChildren<CooldownText>();
    }

    protected override void Start()
    {
        base.Start();
        SetAttackTimer(); // load up first attack
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        UpdateAttackTimer();
        if (attackTimer <= 0f && !isAttacking)
        {
            ChooseAttack();
            ExecuteAttack();
            SetAttackTimer();
        }

        SetColor();
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

    void ChooseAttack()
    {
        int attackType = Random.Range(0, System.Enum.GetValues(typeof(AttackPatterns)).Length);
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
            isAttacking = true;
            anim.SetTrigger("Attack1");
        }
        if (attackPattern == AttackPatterns.Basic2)
        {
            isAttacking = true;
            anim.SetTrigger("Attack2");
        }
    }

    void UpdateAttackTimer()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer < 0f)
        {
            attackTimer = 0f;
        }
        cdText.SetText(attackTimer);
    }

    void SetAttackTimer()
    {
        if (attackPattern == AttackPatterns.Basic1)
        {
            attackTimer = GetAttack1AnimationLength();
        }
        else if (attackPattern == AttackPatterns.Basic2)
        {
            attackTimer = GetAttack2AnimationLength();
        }

        if (health / 100 >= 0.75f)
        {
            attackTimer += 1f;
        }
        else if (health / 100 >= 0.5f)
        {
            attackTimer += 0.5f;
        }
        else if (health / 100 < 0.5f)
        {
            attackTimer += 0.1f;
        }
    }

    float GetAttack1AnimationLength()
    {
        const float TELEGRAPH_FRAMES = 6f;
        const float TELEGRAPH_ANIMATION_SPEED = 0.1f;
        const float ATTACK_FRAMES = 8f;
        const float ATTACK_ANIMATION_SPEED = 0.2f;
        float telegraphLength = (float)(TELEGRAPH_FRAMES / 60.00 / TELEGRAPH_ANIMATION_SPEED);
        float attackLength = (float)(ATTACK_FRAMES / 60.00 / ATTACK_ANIMATION_SPEED);
        return telegraphLength + attackLength;
    }

    float GetAttack2AnimationLength()
    {
        const float TELEGRAPH_FRAMES = 6f;
        const float TELEGRAPH_ANIMATION_SPEED = 0.1f;
        const float ATTACK_FRAMES = 9f;
        const float ATTACK_ANIMATION_SPEED = 0.2f;
        float telegraphLength = (float)(TELEGRAPH_FRAMES / 60.00 / TELEGRAPH_ANIMATION_SPEED);
        float attackLength = (float)(ATTACK_FRAMES / 60.00 / ATTACK_ANIMATION_SPEED);
        return telegraphLength + attackLength;
    }

    protected override void Die()
    {
        base.Die();
        OnDeath?.Invoke();
        audio.PlayDeath();
        spriteR.color = Color.clear;
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }

    void SetColor()
    {
        float healthPct = (float)health / (float)MAX_HEALTH;
        if (healthPct >= 0.25 && healthPct <= 0.5)
        {
            spriteR.color = Color.yellow;
        }
        else if (healthPct < 0.25)
        {
            spriteR.color = Color.red;
        }
    }

    void HandleHitSuccess()
    {
        if (attackPattern == AttackPatterns.Basic1)
        {
            audio.PlayLowImpact();
        }
        if (attackPattern == AttackPatterns.Basic2)
        {
            audio.PlayHighImpact();
        }
    }
}

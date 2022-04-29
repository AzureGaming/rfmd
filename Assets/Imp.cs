using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Enemy
{
    [SerializeField] GameObject bloodSplatPrefab;
    [SerializeField] GameObject summonCirclePrefab;
    HealthBar healthBar;
    Animator anim;
    new Enemy1Audio audio;
    Vector3 startPos;
    Vector3 targetPos;

    const int MAX_HEALTH = 100;
    const float TELEGRAPH_FRAMES = 10.00f;
    const float TELEGRAPH_ANIMATION_SPEED = 0.2f;
    const float ATTACK_FRAMES = 17.00f;
    const float ATTACK_ANIMATION_SPEED = 0.2f;
    const AttackType ATTACK_TYPE = AttackType.Low;

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
        healthBar = GetComponentInChildren<HealthBar>();
        anim = GetComponent<Animator>();
        audio = GetComponent<Enemy1Audio>();
        targetPos = GameObject.FindGameObjectWithTag("ImpTarget").transform.position;
        startPos = transform.position;
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
        OnSpawned?.Invoke(this);

        animationLength = GetAnimationLength();
    }

    private void Update()
    {
        if (shouldAttack)
        {
            shouldAttack = false;
            isAttacking = true;
            anim.SetTrigger("Attack");
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public override void Attack()
    {
        if (!isAttacking)
        {
            shouldAttack = true;
        }
    }

    public void CompleteAttack()
    {
        isAttacking = false;
        shouldAttack = false;
        OnFinishAttackAnimation?.Invoke(this);
    }

    public void CheckHit()
    {
        OnImpact?.Invoke(ATTACK_TYPE, this);
    }

    public override void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        if (health > 0)
        {
            GetComponent<FlashRed>()?.RunRoutine();
        }
    }

    public override void Die()
    {
        OnDeath.Invoke(this);
        //audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }

    public void SpawnSummonCircle()
    {
        SummonCircle summonCircle = Instantiate(summonCirclePrefab, targetPos, Quaternion.identity).GetComponent<SummonCircle>();
        summonCircle.SetImp(this);
    }

    float GetAnimationLength()
    {
        //AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        //AnimationClip clip = Array.Find(clips, (AnimationClip clip) => clip.name == "Telegraph");
        float telegraphLength = (float)(TELEGRAPH_FRAMES / 60.00 / TELEGRAPH_ANIMATION_SPEED);
        float attackLength = (float)(ATTACK_FRAMES / 60.00 / ATTACK_ANIMATION_SPEED);
        // this is rounded
        return telegraphLength + attackLength;
    }

    void HandleHitSuccess()
    {
        if (isAttacking)
        {
            //audio.PlayImpact();
        }
    }

    void SetHealth(int val)
    {
        health = val;
        healthBar?.SetHealth(health);
    }
}

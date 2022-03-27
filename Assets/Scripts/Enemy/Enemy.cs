using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void TakeDamage(int damage);
    public static TakeDamage OnTakeDamage;
    public delegate void Death();
    public static Death OnDeath;

    public delegate void AttackHigh();
    public static AttackHigh OnAttackHigh;
    public delegate void HitHigh();
    public static HitHigh OnHitHigh;

    public delegate void AttackLow();
    public static AttackLow OnAttackLow;
    public delegate void HitLow();
    public static HitLow OnHitLow;

    public EnemyAudio audio;
    [HideInInspector]
    public bool isTelegraphing;
    [HideInInspector]
    public bool isHitting;
    public int health;

    [SerializeField] GameObject bloodSplatPrefab;
    [SerializeField] EnemyAnimations anims;

    const int MAX_HEALTH = 150;
    float fakeoutSpeed = 0.07f;
    float fakeoutReverseSpeed = 0.2f;

    //float[] DELAY_RANGE_LEVEL_0 = new float[] { 3f, 5f };
    //float[] DELAY_RANGE_LEVEL_1 = new float[] { 2f, 4f };
    //float[] DELAY_RANGE_LEVEL_2 = new float[] { 1f, 3f };
    //float[] DELAY_RANGE_LEVEL_3 = new float[] { 1f, 2f };
    //float[] DELAY_RANGE_LEVEL_4 = new float[] { 1f, 2f };

    //float[] TELEGRAPH_RANGE_LEVEL_1 = new float[] { 0.08f, 0.08f };
    //float[] TELEGRAPH_RANGE_LEVEL_2 = new float[] { 0.1f, 0.2f };
    //float[] TELEGRAPH_RANGE_LEVEL_3 = new float[] { 0.1f, 0.2f };
    //float[] TELEGRAPH_RANGE_LEVEL_4 = new float[] { 0.1f, 0.2f };

    const float TELEGRAPH_LEVEL_0 = 0.05f;

    const float ATTACK_DELAY_MAX_LEVEL_0 = 5f;
    const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;

    const float ATTACK_SPEED = 0.2f;

    GameManager gameManager;
    Coroutine attackRoutine;
    HealthBar healthBar;

    bool isFakeAttack = false;

    float attackType;
    float attackTimer;
    bool isAttacking;

    private void OnEnable()
    {
        OnHitHigh += audio.PlayHighAttackHit;
        OnHitLow += audio.PlayLowAttackHit;
        OnTakeDamage += HandleTakeDamage;
        Player.OnDeath += StopAttacking;
    }

    private void OnDisable()
    {
        OnHitHigh -= audio.PlayHighAttackHit;
        OnHitLow -= audio.PlayLowAttackHit;
        OnTakeDamage -= HandleTakeDamage;
        Player.OnDeath -= StopAttacking;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
    }

    private void Update()
    {
        HandleAttacking();
    }

    void HandleAttacking()
    {
        attackTimer -= Time.deltaTime;
        if (isAttacking)
        {
            return;
        }
        if (attackTimer <= 0)
        {
            attackTimer = GetAttackTimer();
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    void HandleTakeDamage(int damage)
    {
        SetHealth(health - damage);
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackType = Random.Range(0, 2);

        yield return StartCoroutine(Telegraph());
        isFakeAttack = gameManager.level == 2;

        if (isFakeAttack)
        {
            anims.StopTelegraphAttack(1f);
        }
        else
        {
            yield return StartCoroutine(Hit());
        }

        isAttacking = false;
    }

    IEnumerator Telegraph()
    {
        isTelegraphing = true;
        if (attackType == 0)
        {
            anims.PlayTelegraphAttackHigh(GetTelegraphSpeed());
        }
        if (attackType == 1)
        {
            anims.PlayTelegraphAttackLow(GetTelegraphSpeed());
        }
        yield return new WaitUntil(() => !isTelegraphing);
    }

    IEnumerator Hit()
    {
        isHitting = true;
        if (attackType == 0)
        {
            anims.PlayAttackHigh(GetAttackSpeed());
        }
        if (attackType == 1)
        {
            anims.PlayAttackLow(GetAttackSpeed());
        }
        yield return new WaitUntil(() => !isHitting);
    }


    float GetAttackTimer()
    {
        switch (gameManager.level)
        {
            case 0:
                return Random.Range(ATTACK_DELAY_MIN_LEVEL_0, ATTACK_DELAY_MAX_LEVEL_0);
            default:
                return Random.Range(ATTACK_DELAY_MIN_LEVEL_0, ATTACK_DELAY_MAX_LEVEL_0);
        }
    }

    float GetTelegraphSpeed()
    {
        switch (gameManager.level)
        {
            case 0:
                return TELEGRAPH_LEVEL_0;
            default:
                return TELEGRAPH_LEVEL_0;
        }
    }

    float GetAttackSpeed()
    {
        return ATTACK_SPEED;
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
        Destroy(gameObject);
    }

    void StopAttacking()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        isAttacking = true; // TODO: refactor hack
    }
}

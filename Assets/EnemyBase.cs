using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //public delegate void Death();
    //public static Death OnDeath;

    //public delegate void AttackHigh(Enemy self);
    //public static AttackHigh OnAttackHigh;
    //public delegate void HitHigh();
    //public static HitHigh OnHitHigh;
    //public delegate void MissHigh();
    //public static MissHigh OnMissHigh;

    //public delegate void AttackLow(Enemy self);
    //public static AttackLow OnAttackLow;
    //public delegate void HitLow();
    //public static HitLow OnHitLow;
    //public delegate void MissLow();
    //public static MissLow OnMissLow;

    //public int health;
    //[HideInInspector]
    //public bool isTelegraphing;
    //[HideInInspector]
    //public bool isHitting;
    //[HideInInspector]
    //public bool isReverseTelegraphing;

    //[SerializeField] GameObject bloodSplatPrefab;
    //[SerializeField] EnemyAnimations anims;

    //const int MAX_HEALTH = 10;

    ////float[] DELAY_RANGE_LEVEL_0 = new float[] { 3f, 5f };
    ////float[] DELAY_RANGE_LEVEL_1 = new float[] { 2f, 4f };
    ////float[] DELAY_RANGE_LEVEL_2 = new float[] { 1f, 3f };
    ////float[] DELAY_RANGE_LEVEL_3 = new float[] { 1f, 2f };
    ////float[] DELAY_RANGE_LEVEL_4 = new float[] { 1f, 2f };

    ////float[] TELEGRAPH_RANGE_LEVEL_2 = new float[] { 0.1f, 0.2f };
    ////float[] TELEGRAPH_RANGE_LEVEL_3 = new float[] { 0.1f, 0.2f };
    ////float[] TELEGRAPH_RANGE_LEVEL_4 = new float[] { 0.1f, 0.2f };

    //const float TRICK_TELEGRAPH_SPEED = 0.15f;
    //const float TELEGRAPH_SPEED_LEVEL_0 = 0.05f;
    //const float TELEGRAPH_SPEED_LEVEL_1 = 0.08f;

    ////const float TRICK_ATTACK_DELAY = 0.2f;
    //const float FIRST_ATTACK_DELAY_MAX_LEVEL_0 = 5f;
    //const float FIRST_ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    //const float ATTACK_DELAY_MAX_LEVEL_0 = 4f;
    //const float ATTACK_DELAY_MIN_LEVEL_0 = 3f;
    //const float ATTACK_DELAY_MAX_LEVEL_1 = 3f;
    //const float ATTACK_DELAY_MIN_LEVEL_1 = 2f;

    ////const float TRICK_ATTACK_SPEED = 1f;
    //const float ATTACK_SPEED = 0.2f;

    //GameManager gameManager;
    //Coroutine attackRoutine;
    //HealthBar healthBar;
    //SpriteRenderer spriteR;
    //Color origColor;

    //float attackType;
    //float attackTimer;
    //float lastTelegraphSpeed;
    //float lastAttackTimer;
    //bool isAttacking;
    //bool isFakeAttack = false;

    //private void OnEnable()
    //{
    //    //OnHitHigh += audio.PlayHighAttackHit; // invoked from player 
    //    //OnHitLow += audio.PlayLowAttackHit; // invoked from player
    //    //OnMissLow += audio.PlayAttackLow; // invoked from player
    //    //OnMissHigh += audio.PlayAttackHigh; // invoked from player
    //    Player.OnDeath += StopAttacking;
    //    GameManager.OnDamageEnemy += TakeDamage;
    //}

    //private void OnDisable()
    //{
    //    //OnHitHigh -= audio.PlayHighAttackHit;
    //    //OnHitLow -= audio.PlayLowAttackHit;
    //    //OnMissLow -= audio.PlayAttackLow;
    //    //OnMissHigh -= audio.PlayAttackHigh;
    //    Player.OnDeath -= StopAttacking;
    //    GameManager.OnDamageEnemy -= TakeDamage;
    //}

    //private void Awake()
    //{
    //    gameManager = FindObjectOfType<GameManager>();
    //    healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
    //    spriteR = GetComponent<SpriteRenderer>();
    //    origColor = spriteR.color;
    //}

    //private void Start()
    //{
    //    SetHealth(MAX_HEALTH);
    //    healthBar.SetMaxHealth(MAX_HEALTH);
    //}


    //void HandleAttacking()
    //{
    //    if (isAttacking)
    //    {
    //        return;
    //    }
    //    // only start counting when the last attack finishes
    //    attackTimer -= Time.deltaTime;

    //    if (attackTimer <= 0)
    //    {
    //        attackRoutine = StartCoroutine(AttackRoutine());
    //    }
    //}

    //public void TakeDamage(int damage)
    //{
    //    SetHealth(health - damage);
    //    if (health <= 0)
    //    {
    //        Die();
    //    }
    //}

    //public IEnumerator AttackRoutine()
    //{
    //    isAttacking = true;
    //    attackType = Random.Range(0, 2);

    //    yield return StartCoroutine(Telegraph());
    //    SetFakeAttack();

    //    if (isFakeAttack)
    //    {
    //        yield return StartCoroutine(FakeoutHit());
    //    }
    //    else
    //    {
    //        yield return StartCoroutine(Hit());
    //    }

    //    isAttacking = false;
    //    isFakeAttack = false;

    //    attackTimer = GetAttackTimer();
    //    Debug.Log($"Set Attack timer: {attackTimer}");
    //}

    //IEnumerator Telegraph()
    //{
    //    isTelegraphing = true;
    //    if (attackType == 0)
    //    {
    //        anims.PlayTelegraphAttackHigh(GetTelegraphSpeed());
    //    }
    //    if (attackType == 1)
    //    {
    //        anims.PlayTelegraphAttackLow(GetTelegraphSpeed());
    //    }
    //    yield return new WaitUntil(() => !isTelegraphing);
    //}

    //IEnumerator Hit()
    //{
    //    isHitting = true;
    //    if (attackType == 0)
    //    {
    //        anims.PlayAttackHigh(GetAttackSpeed());
    //    }
    //    if (attackType == 1)
    //    {
    //        anims.PlayAttackLow(GetAttackSpeed());
    //    }
    //    yield return new WaitUntil(() => !isHitting);
    //}

    //IEnumerator FakeoutHit()
    //{
    //    isReverseTelegraphing = true;
    //    anims.PlayReverseTelegraph(GetAttackSpeed());
    //    yield return new WaitUntil(() => !isReverseTelegraphing);
    //    yield return StartCoroutine(Telegraph());
    //    yield return StartCoroutine(Hit());
    //}


    //float GetAttackTimer()
    //{
    //    int level = gameManager.level;

    //    if (level == 0)
    //    {
    //        if (gameManager.isFirstEnemy) // give player more leeway when starting the game
    //        {
    //            lastAttackTimer = Random.Range(FIRST_ATTACK_DELAY_MIN_LEVEL_0, FIRST_ATTACK_DELAY_MAX_LEVEL_0);
    //        }
    //        else
    //        {
    //            lastAttackTimer = Random.Range(ATTACK_DELAY_MIN_LEVEL_0, ATTACK_DELAY_MAX_LEVEL_0);
    //        }
    //    }
    //    else
    //    {
    //        lastAttackTimer = Random.Range(ATTACK_DELAY_MIN_LEVEL_1, ATTACK_DELAY_MAX_LEVEL_1);
    //    }
    //    return lastAttackTimer;
    //}

    //float GetTelegraphSpeed()
    //{
    //    int level = gameManager.level;

    //    if (isFakeAttack)
    //    {
    //        lastTelegraphSpeed = TRICK_TELEGRAPH_SPEED;
    //        return lastTelegraphSpeed;
    //    }

    //    if (level == 0)
    //    {
    //        lastTelegraphSpeed = TELEGRAPH_SPEED_LEVEL_0;
    //    }
    //    else
    //    {
    //        lastTelegraphSpeed = TELEGRAPH_SPEED_LEVEL_1;
    //    }

    //    return lastTelegraphSpeed;
    //}

    //void SetFakeAttack()
    //{
    //    if (gameManager.level >= 2)
    //    {
    //        isFakeAttack = Random.Range(0f, 1f) >= 0.5f;
    //    }
    //    else
    //    {
    //        isFakeAttack = false;
    //    }
    //}

    //float GetAttackSpeed()
    //{
    //    return ATTACK_SPEED;
    //}

    //void SetHealth(int val)
    //{
    //    health = val;
    //    healthBar.SetHealth(health);
    //}

    //void Die()
    //{
    //    OnDeath.Invoke();
    //    //audio.PlayDeath();
    //    Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
    //    Destroy(gameObject);
    //}

    //void StopAttacking()
    //{
    //    if (attackRoutine != null)
    //    {
    //        StopCoroutine(attackRoutine);
    //    }
    //    isAttacking = true; // TODO: refactor hack
    //}
}

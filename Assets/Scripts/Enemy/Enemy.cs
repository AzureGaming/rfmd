using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void TakeDamage(int damage);
    public static TakeDamage OnTakeDamage;
    public delegate void Death();
    public static Death OnDeath;

    public delegate void HighAttackHit();
    public static HighAttackHit OnHighAttackHit;
    public delegate void LowAttackHit();
    public static LowAttackHit OnLowAttackHit;

    public EnemyAudio audio;

    [SerializeField] GameObject bloodSplatPrefab;

    [SerializeField] EnemyAnimations anims;

    const int MAX_HEALTH = 150;
    float fakeoutSpeed = 0.07f;
    float fakeoutReverseSpeed = 0.2f;

    float[] DELAY_RANGE_LEVEL_0 = new float[] { 3f, 5f };
    float[] DELAY_RANGE_LEVEL_1 = new float[] { 2f, 4f };
    float[] DELAY_RANGE_LEVEL_2 = new float[] { 1f, 3f };
    float[] DELAY_RANGE_LEVEL_3 = new float[] { 1f, 2f };
    float[] DELAY_RANGE_LEVEL_4 = new float[] { 1f, 2f };

    float[] TELEGRAPH_RANGE_LEVEL_0 = new float[] { 0.05f, 0.05f };
    float[] TELEGRAPH_RANGE_LEVEL_1 = new float[] { 0.08f, 0.08f };
    float[] TELEGRAPH_RANGE_LEVEL_2 = new float[] { 0.1f, 0.2f };
    float[] TELEGRAPH_RANGE_LEVEL_3 = new float[] { 0.1f, 0.2f };
    float[] TELEGRAPH_RANGE_LEVEL_4 = new float[] { 0.1f, 0.2f };

    float ATTACK_SPEED_LEVEL_0 = 0.2f;
    float ATTACK_SPEED_LEVEL_1 = 0.2f;
    float ATTACK_SPEED_LEVEL_2 = 0.2f;
    float ATTACK_SPEED_LEVEL_3 = 0.2f;
    float ATTACK_SPEED_LEVEL_4 = 0.2f;

    float[] telegraphRange;
    float[] delayRange;

    float attackSpeed;
    GameManager gameManager;
    Coroutine attackRoutine;
    SpriteRenderer spriteR;
    HealthBar healthBar;

    bool isFakeAttackEnabled = false;
    bool isFakeAttack = false;

    float attackType;
    float telegraphSpeed;
    int health;

    private void OnEnable()
    {
        OnHighAttackHit += audio.PlayHighAttackHit;
        OnLowAttackHit += audio.PlayLowAttackHit;
        OnTakeDamage += GetHit;
    }

    private void OnDisable()
    {
        OnHighAttackHit -= audio.PlayHighAttackHit;
        OnLowAttackHit -= audio.PlayLowAttackHit;
        OnTakeDamage -= GetHit;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteR = GetComponent<SpriteRenderer>();
        healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
        UpdateAnimationSpeeds();

        float attackDelay = Random.Range(delayRange[0], delayRange[1]);

        attackRoutine = StartCoroutine(Attack(attackDelay));
    }

    public void UpdateAnimationSpeeds()
    {
        SetAnimationSpeeds(gameManager.level);
    }

    public void AttackHighTelegraphEvent()
    {
        float attackDelay = Random.Range(delayRange[0], delayRange[1]);

        if (isFakeAttack)
        {
            attackDelay = 0.5f;
            anims.StopTelegraphAttack(fakeoutReverseSpeed);
        }
        else
        {
            //attackTiming.StartHighAttackTimer();
            audio.PlayAttackHigh();
            anims.PlayAttackHigh(attackSpeed);
        }

        attackRoutine = StartCoroutine(Attack(attackDelay));
    }

    public void AttackLowTelegraphEvent()
    {
        float attackDelay = Random.Range(delayRange[0], delayRange[1]);
        if (isFakeAttack)
        {
            attackDelay = 0.5f;
            anims.StopTelegraphAttack(fakeoutReverseSpeed);
        }
        else
        {
            anims.PlayAttackLow(attackSpeed);
        }

        attackRoutine = StartCoroutine(Attack(attackDelay));
    }

    public void AttackHighAnimationEvent()
    {
        Player.OnAttacked?.Invoke("HIGH");
    }

    public void AttackLowAnimationEvent()
    {
        Player.OnAttacked?.Invoke("LOW");
    }

    public void StartAttacking()
    {
        anims.StartCurrent();
        Attack();
    }

    public void StopAttacking()
    {
        StopCoroutine(attackRoutine);
        anims.StopCurrent();
    }

    public void GetHit(int damage)
    {
        SetHealth(health - damage);
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    void Die()
    {
        OnDeath.Invoke();
        audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }

    void SetHealth(int val)
    {
        health = val;
        healthBar.SetHealth(health);
    }

    void Attack()
    {
        attackType = Random.Range(0, 2);
        telegraphSpeed = Random.Range(telegraphRange[0], telegraphRange[1]);
        SetFakeout();

        if (isFakeAttack)
        {
            telegraphSpeed = fakeoutSpeed;
        }

        if (attackType == 0)
        {
            anims.PlayTelegraphAttackHigh(telegraphSpeed);
        }
        else
        {
            anims.PlayTelegraphAttackLow(telegraphSpeed);
        }

    }

    IEnumerator Attack(float delay)
    {
        attackType = Random.Range(0, 2);
        telegraphSpeed = Random.Range(telegraphRange[0], telegraphRange[1]);
        SetFakeout();

        if (isFakeAttack)
        {
            telegraphSpeed = fakeoutSpeed;
        }

        Debug.Log($"Delay attack for {delay} seconds.");
        yield return new WaitForSeconds(delay);

        if (attackType == 0)
        {
            anims.PlayTelegraphAttackHigh(telegraphSpeed);
        }
        else
        {
            anims.PlayTelegraphAttackLow(telegraphSpeed);
        }
    }

    IEnumerator FlashRed()
    {
        float timeElapsed = 0f;
        float totalTime = 0.001f;
        Color origColor = spriteR.color;
        Color redColor = Color.red;
        redColor.a = 50;

        while (timeElapsed <= totalTime)
        {
            if (spriteR.color == origColor)
            {
                spriteR.color = redColor;
            }
            else
            {
                spriteR.color = origColor;
            }
            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.15f);
        }
        spriteR.color = origColor;
    }


    void SetFakeout()
    {
        isFakeAttack = false;
        if (isFakeAttackEnabled)
        {
            float randomVal = Random.value;
            isFakeAttack = randomVal > 0.8f;
            Debug.Log($"Fakeout attack {randomVal}");
        }
    }

    void SetAnimationSpeeds(int level)
    {
        switch (level)
        {
            case 1:
                telegraphRange = TELEGRAPH_RANGE_LEVEL_0;
                attackSpeed = ATTACK_SPEED_LEVEL_0;
                delayRange = DELAY_RANGE_LEVEL_0;
                break;
            case 2:
                telegraphRange = TELEGRAPH_RANGE_LEVEL_1;
                attackSpeed = ATTACK_SPEED_LEVEL_1;
                delayRange = DELAY_RANGE_LEVEL_1;
                break;
            //case 3:
            //    telegraphRange = TELEGRAPH_RANGE_LEVEL_2;
            //    attackSpeed = ATTACK_SPEED_LEVEL_2;
            //    delayRange = DELAY_RANGE_LEVEL_2;
            //    isFakeAttackEnabled = true;
            //    break;
            //case 4:
            //    telegraphRange = level4TelegraphRange;
            //    attackSpeed = level4ATTACK_SPEED;
            //    delayRange = level4DELAY_RANGE;
            //    break;
            //case 5:
            //    telegraphRange = level5TelegraphRange;
            //    attackSpeed = level5ATTACK_SPEED;
            //    delayRange = level5DELAY_RANGE;
            //    break;
            default:
                Debug.LogWarning($"Invalid level {level}.");
                telegraphRange = TELEGRAPH_RANGE_LEVEL_0;
                attackSpeed = ATTACK_SPEED_LEVEL_0;
                delayRange = DELAY_RANGE_LEVEL_0;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void LevelUp(int level);
    public static LevelUp OnLevelUp;

    public delegate void HighAttackHit();
    public static HighAttackHit OnHighAttackHit;
    public delegate void LowAttackHit();
    public static LowAttackHit OnLowAttackHit;

    public EnemyAudio audio;

    [SerializeField] EnemyAnimations anims;

    float fakeoutSpeed = 0.07f;
    float fakeoutReverseSpeed = 0.2f;

    float[] level1DelayRange = new float[] { 3f, 5f };
    float[] level2DelayRange = new float[] { 2f, 4f };
    float[] level3DelayRange = new float[] { 1f, 3f };
    float[] level4DelayRange = new float[] { 1f, 2f };
    float[] level5DelayRange = new float[] { 1f, 2f };

    float[] level1TelegraphRange = new float[] { 0.1f, 0.1f };
    float[] level2TelegraphRange = new float[] { 0.1f, 0.1f };
    float[] level3TelegraphRange = new float[] { 0.1f, 0.2f };
    float[] level4TelegraphRange = new float[] { 0.1f, 0.2f };
    float[] level5TelegraphRange = new float[] { 0.1f, 0.2f };

    float level1AttackSpeed = 0.2f;
    float level2AttackSpeed = 0.2f;
    float level3AttackSpeed = 0.2f;
    float level4AttackSpeed = 0.2f;
    float level5AttackSpeed = 0.2f;

    float[] telegraphRange;
    float[] delayRange;

    float attackSpeed;
    GameManager gameManager;
    Coroutine attackRoutine;
    bool isFakeAttackEnabled = false;
    bool isFakeAttack = false;

    float attackType;
    float telegraphSpeed;

    private void OnEnable()
    {
        OnLevelUp += SetAnimationSpeeds;
        OnHighAttackHit += audio.PlayHighAttackHit;
        OnLowAttackHit += audio.PlayLowAttackHit;
    }

    private void OnDisable()
    {
        OnLevelUp -= SetAnimationSpeeds;
        OnHighAttackHit -= audio.PlayHighAttackHit;
        OnLowAttackHit -= audio.PlayLowAttackHit;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        SetAnimationSpeeds(gameManager.level);

        float attackDelay = Random.Range(delayRange[0], delayRange[1]);
        attackRoutine = StartCoroutine(Attack(attackDelay));
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

    public void Stop()
    {
        StopCoroutine(attackRoutine);
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
                telegraphRange = level1TelegraphRange;
                attackSpeed = level1AttackSpeed;
                delayRange = level1DelayRange;
                break;
            case 2:
                telegraphRange = level2TelegraphRange;
                attackSpeed = level2AttackSpeed;
                delayRange = level2DelayRange;
                break;
            case 3:
                telegraphRange = level3TelegraphRange;
                attackSpeed = level3AttackSpeed;
                delayRange = level3DelayRange;
                isFakeAttackEnabled = true;
                break;
            case 4:
                telegraphRange = level4TelegraphRange;
                attackSpeed = level4AttackSpeed;
                delayRange = level4DelayRange;
                break;
            case 5:
                telegraphRange = level5TelegraphRange;
                attackSpeed = level5AttackSpeed;
                delayRange = level5DelayRange;
                break;
            default:
                Debug.LogWarning($"Invalid level {level}.");
                break;
        }
    }
}

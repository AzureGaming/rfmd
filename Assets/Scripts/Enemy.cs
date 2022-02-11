using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void LevelUp(int level);
    public static LevelUp OnLevelUp;

    [SerializeField] EnemyAnimations anims;

    float fakeoutSpeed = 0.07f;
    float fakeoutReverseSpeed = 0.2f;

    float[] level1DelayRange = new float[] { 3f, 5f };
    float[] level2DelayRange = new float[] { 2f, 4f };
    float[] level3DelayRange = new float[] { 1f, 3f };

    float[] level1TelegraphRange = new float[] { 0.1f, 0.1f };
    float[] level2TelegraphRange = new float[] { 0.1f, 0.1f };
    float[] level3TelegraphRange = new float[] { 0.1f, 0.2f };

    float level1AttackSpeed = 0.2f;
    float level2AttackSpeed = 0.2f;
    float level3AttackSpeed = 0.2f;

    float[] telegraphRange;
    float[] delayRange;

    float attackSpeed;
    GameManager gameManager;
    bool isFakeAttackEnabled = false;
    bool isFakeAttack = false;

    float attackType;
    float telegraphSpeed;

    private void OnEnable()
    {
        OnLevelUp += SetAnimationSpeeds;
    }

    private void OnDisable()
    {
        OnLevelUp -= SetAnimationSpeeds;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        SetAnimationSpeeds(gameManager.level);

        float attackDelay = Random.Range(delayRange[0], delayRange[1]);
        StartCoroutine(Attack(attackDelay));
    }

    public IEnumerator AttackHighTelegraphEvent()
    {
        float attackDelay = Random.Range(delayRange[0], delayRange[1]);

        if (isFakeAttack)
        {
            attackDelay = 0.5f;
            anims.StopTelegraphAttack(fakeoutReverseSpeed);
        }
        else
        {
            anims.PlayAttackHigh(attackSpeed);
        }

        StartCoroutine(Attack(attackDelay));

        yield break;
    }

    public IEnumerator AttackLowTelegraphEvent()
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

        StartCoroutine(Attack(attackDelay));

        yield break;
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
        StopCoroutine("Attack");
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
            default:
                Debug.LogError($"Invalid level {level}.");
                break;
        }
    }
}

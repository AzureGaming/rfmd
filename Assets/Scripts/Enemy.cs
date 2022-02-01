using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyAnimations anims;

    GameManager gameManager;

    bool isFakeAttack;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(Attack());
    }

    public IEnumerator AttackHighTelegraphEvent()
    {
        yield return new WaitForSeconds(0.5f);
        if (isFakeAttack)
        {
            anims.StopAttack();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            anims.PlayAttackHigh();
            yield return new WaitForSeconds(1.5f);
        }

        StartCoroutine(Attack());
    }

    public IEnumerator AttackLowTelegraphEvent()
    {
        yield return new WaitForSeconds(0.5f);
        if (isFakeAttack)
        {
            anims.StopAttack();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            anims.PlayAttackLow();
            yield return new WaitForSeconds(1.5f);
        }
        StartCoroutine(Attack());
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
        StopCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float randomAttack = Random.Range(0, 2);
        float seconds = 0f;

        isFakeAttack = Random.value > 0.8f; // fake out attack

        if (gameManager.level == 1)
        {
            seconds = 2.5f;
        }
        else if (gameManager.level == 2)
        {
            seconds = 1.75f;
        }
        else if (gameManager.level == 3)
        {
            seconds = 1f;
        }

        yield return new WaitForSeconds(seconds);

        if (randomAttack == 0)
        {
            anims.PlayTelegraphAttackHigh();
        }
        else
        {
            anims.PlayTelegraphAttackLow();
        }
    }
}

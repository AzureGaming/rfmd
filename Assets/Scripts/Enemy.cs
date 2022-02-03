using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyAnimations anims;

    bool isPlayerAlive = true;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    public IEnumerator AttackHighTelegraphEvent()
    {
        yield return new WaitForSeconds(0.5f);
        anims.PlayAttackHigh();
    }

    public IEnumerator AttackLowTelegraphEvent()
    {
        yield return new WaitForSeconds(0.5f);
        anims.PlayAttackLow();
    }

    public void AttackHighAnimationEvent()
    {
        Player.OnAttacked?.Invoke("HIGH");
    }

    public void AttackLowAnimationEvent()
    {
        Player.OnAttacked?.Invoke("LOW");
    }

    IEnumerator AttackRoutine()
    {
         while(isPlayerAlive)
        {
            float seconds = 2f;
            float randomAttack = Random.Range(0, 2);

            yield return new WaitForSeconds(seconds);

            if (randomAttack == 0)
            {
                TelegraphAttackHigh();
            } else
            {
                TelegraphAttackLow();
            }
        }
    }

    void TelegraphAttackHigh()
    {
        anims.PlayTelegraphAttackHigh();
    }

    void TelegraphAttackLow()
    {
        anims.PlayTelegraphAttackLow();
    }
}
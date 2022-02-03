using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTelegraphAttackHigh()
    {
        animator.SetTrigger("Telegraph Attack High");
    }

    public void PlayTelegraphAttackLow()
    {
        animator.SetTrigger("Telegraph Attack Low");
    }

    public void PlayAttackHigh()
    {
        animator.SetTrigger("Attack High");
    }

    public void PlayAttackLow()
    {
        animator.SetTrigger("Attack Low");
    }

    public void StopAttack()
    {
        animator.SetTrigger("Stop Attack");
    }
}

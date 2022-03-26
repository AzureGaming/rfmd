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

    public void PlayTelegraphAttackHigh(float speed)
    {
        SetAnimationSpeed(speed);
        animator.SetTrigger("Telegraph Attack High");
    }

    public void PlayTelegraphAttackLow(float speed)
    {
        SetAnimationSpeed(speed);
        animator.SetTrigger("Telegraph Attack Low");
    }

    public void PlayAttackHigh(float speed)
    {
        SetAnimationSpeed(speed);
        animator.SetTrigger("Attack High");
    }

    public void PlayAttackLow(float speed)
    {
        SetAnimationSpeed(speed);
        animator.SetTrigger("Attack Low");
    }

    public void StopTelegraphAttack(float speed)
    {
        SetAnimationSpeed(speed);
        animator.SetTrigger("Stop Fakeout");
    }

    public void StopCurrent()
    {
        animator.enabled = false;
    }

    public void StartCurrent()
    {
        animator.enabled = true;
    }

    //public void StopAttack()
    //{
    //    animator.SetTrigger("Stop Attack");
    //}

    public void SetAnimationSpeed(float val)
    {
        animator.SetFloat("Animation Speed", val);
    }
}

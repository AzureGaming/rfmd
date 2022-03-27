using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// there are some functions in EnemyAudio that are also animation events..

public class EnemyAnimationEvents : MonoBehaviour
{
    Enemy enemyRef;
    private void Awake()
    {
        enemyRef = GetComponent<Enemy>();
    }

    public void TelegraphAnimationComplete()
    {
        enemyRef.isTelegraphing = false;
    }

    public void AttackAnimationComplete()
    {
        enemyRef.isHitting = false;
    }

    public void AttackHighAnimationImpact()
    {
        Enemy.OnAttackHigh?.Invoke();
    }

    public void AttackLowAnimationImpact()
    {
        Enemy.OnAttackLow?.Invoke();
    }
}

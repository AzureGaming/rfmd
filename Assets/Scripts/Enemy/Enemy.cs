using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum AttackType
    {
        High,
        Low
    }

    public delegate void Spawned(Enemy self);
    public static Spawned OnSpawned;
    public delegate void Death(Enemy self);
    public static Death OnDeath;
    public delegate void Impact(AttackType attackType, Enemy self);
    public static Impact OnImpact;
    public delegate void FinishAttackAnimation(Enemy self);
    public static FinishAttackAnimation OnFinishAttackAnimation;

    public int health;
    public float animationLength;
    public bool isAttacking = false;
    public bool shouldAttack = false;

    protected void OnEnable()
    {
        Player.OnDodged += FlashRed;
    }

    private void OnDisable()
    {
        Player.OnDodged -= FlashRed;
    }

    public virtual void Attack() { }
    public virtual void TakeDamage(int damage) { }
    public virtual void Die() { }

    void FlashRed(Enemy enemyRef)
    {
        if (this == enemyRef)
        {
            GetComponent<FlashRed>().RunRoutine();
        }
    }
}

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

    public int health;

    public virtual void Attack() { }
    public virtual void TakeDamage(int damage) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public enum AttackType
    {
        High,
        Low
    }

    public virtual IEnumerator Attack() { yield break; }
    public virtual void TakeDamage(int damage) { }
}

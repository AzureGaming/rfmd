using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public delegate void Spawn(Enemy1 self);
    public static Spawn OnSpawn;
    public delegate void Impact();
    public static Impact OnImpact;
    public delegate void Death();
    public static Death OnDeath;

    [SerializeField] GameObject bloodSplatPrefab;
    HealthBar healthBar;
    Animator anim;
    EnemyAudio audio;
     
    const int MAX_HEALTH = 10;
    int health;
    bool isAttacking;

    private void Awake()
    {
        healthBar = GameObject.FindGameObjectWithTag("EnemyHealthBar").GetComponent<HealthBar>();
        anim = GetComponent<Animator>();
        audio = GetComponent<EnemyAudio>();
    }

    private void Start()
    {
        SetHealth(MAX_HEALTH);
        healthBar.SetMaxHealth(MAX_HEALTH);
        OnSpawn?.Invoke(this);
    }

    public void CheckHit()
    {
        isAttacking = false;
        OnImpact?.Invoke();
    }

    public IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitUntil(() => !isAttacking);
    }

    public void TakeDamage(int damage)
    {
        SetHealth(health - damage);
        if (health <= 0)
        {
            Die();
        }
    }

    void SetHealth(int val)
    {
        health = val;
        healthBar.SetHealth(health);
    }

    void Die()
    {
        OnDeath.Invoke();
        audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }
}

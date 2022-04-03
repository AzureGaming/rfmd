using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Boss
{
    [SerializeField] GameObject bloodSplatPrefab;
    Animator anim;
    SpriteRenderer spriteR;
    new Boss1Audio audio;

    const float DESTROY_DELAY = 0.5f;

    private void OnEnable()
    {
        Player.OnHit += HandlePlayerHit;
        GameManager.OnDamageBoss += TakeDamage;
    }

    private void OnDisable()
    {
        Player.OnHit -= HandlePlayerHit;
        GameManager.OnDamageBoss -= TakeDamage;
    }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        audio = GetComponent<Boss1Audio>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void CheckHit()
    {
        OnImpact?.Invoke(attackType);
    }

    public void DoneAttack()
    {
        isAttacking = false;
        if (attackType == AttackType.Low)
        {
            //audio.PlayLowAttack();
        }
        if (attackType == AttackType.High)
        {
            //audio.PlayHighAttack();
        }
    }


    public override IEnumerator Attack()
    {
        isAttacking = true;
        attackType = (AttackType)Random.Range(0, 2);
        if (attackType == AttackType.Low)
        {
            anim.SetTrigger("Attack1");
            //audio.PlayLowTelegraph();
        }
        if (attackType == AttackType.High)
        {
            anim.SetTrigger("Attack2");
            //audio.PlayHighTelegraph();
        }
        yield return new WaitUntil(() => !isAttacking);


        //loop
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attack());
    }

    protected override IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(Attack());
    }

    protected override void Die()
    {
        OnDeath?.Invoke();
        audio.PlayDeath();
        spriteR.color = Color.clear;
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        StartCoroutine(Destroy(DESTROY_DELAY));
    }

    void HandlePlayerHit()
    {
        if (attackType == AttackType.Low)
        {
            //audio.PlayLowImpact();
        }
        if (attackType == AttackType.High)
        {
            //audio.PlayHighImpact();
        }
    }

    IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

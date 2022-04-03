using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Boss
{
    [SerializeField] GameObject bloodSplatPrefab;
    Animator anim;
    //EnemyAudio audio;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        //audio = GetComponent<EnemyAudio>();
    }

    public void CheckHit()
    {
        OnImpact?.Invoke(attackType);
    }

    public override IEnumerator Attack()
    {
        isAttacking = true;
        attackType = (AttackType)Random.Range(0, 2);
        if (attackType == AttackType.Low)
        {
            anim.SetTrigger("Attack1");
        }
        if (attackType == AttackType.High)
        {
            anim.SetTrigger("Attack2");
        }
        yield return new WaitUntil(() => !isAttacking);
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
        //audio.PlayDeath();
        Instantiate(bloodSplatPrefab, transform.position - new Vector3(0.5f, 0f), Quaternion.identity, Camera.main.transform);
        Destroy(gameObject);
    }
}

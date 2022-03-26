using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnAttackLowAnimationEventStart()
    {
        audioManager.Play("Enemy_Attack_Low");
    }

    public void PlayAttackHigh()
    {
        audioManager.Play("Enemy_Attack_High");
    }

    public void OnAttackLowTelegraphAnimationEventStart()
    {
        audioManager.Play("Enemy_Telegraph_Low");
    }

    public void OnAttackHighTelegraphAnimationEventStart()
    {
        audioManager.Play("Enemy_Telegraph_High");
    }

    public void OnAttackHighMissAnimationEventStart()
    {
        audioManager.Play("Enemy_Attack_High_Miss");
    }

    public void OnAttackLowMissAnimationEventStart()
    {
        audioManager.Play("Enemy_Attack_Low_Miss");
    }

    public void PlayHighAttackHit()
    {
        audioManager.Play("Enemy_Attack_High_Impact");
    }

    public void PlayLowAttackHit()
    {
        audioManager.Play("Enemy_Attack_Low_Impact");
    }

    public void PlayDeath()
    {
        audioManager.Play("Enemy_Death");
    }
}

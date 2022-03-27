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

    public void PlayAttackLow()
    {
        audioManager.Play("Enemy_Attack_Low");
    }

    public void PlayAttackHigh()
    {
        audioManager.Play("Enemy_Attack_High");
    }

    public void PlayTelegraphLow()
    {
        audioManager.Play("Enemy_Telegraph_Low");
    }

    public void PlayTelegraphHigh()
    {
        audioManager.Play("Enemy_Telegraph_High");
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

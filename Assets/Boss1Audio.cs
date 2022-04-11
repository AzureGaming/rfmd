using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Audio : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    public void PlayHighAttack()
    {
        audioManager.Play("Attack_High");
    }

    public void PlayHighTelegraph()
    {
        audioManager.Play("Telegraph_High");
    }

    public void PlayHighImpact()
    {
        audioManager.Play("High_Impact");
    }

    public void PlayLowAttack()
    {
        audioManager.Play("Attack_Low");
    }

    public void PlayLowTelegraph()
    {
        audioManager.Play("Telegraph_Low");
    }

    public void PlayLowImpact()
    {
        audioManager.Play("Low_Impact");
    }

    public void PlayDeath()
    {
        audioManager.Play("Death", transform.position);
    }
}

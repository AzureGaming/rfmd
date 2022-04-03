using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Audio : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    public void PlayAttack()
    {
        audioManager.Play("Attack");
    }

    public void PlayTelegraph()
    {
        audioManager.Play("Telegraph");
    }

    public void PlayImpact()
    {
        audioManager.Play("Impact");
    }

    public void PlayDeath()
    {
        audioManager.Play("Death");
    }
}

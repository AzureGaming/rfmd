using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioManager audioManager;

    Action stopRun;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    public void PlaySlide()
    {
        audioManager.Play("Slide");
    }

    public void PlayDodge()
    {
        audioManager.Play("Dodge");
    }

    public void PlayJump()
    {
        audioManager.Play("Jump");
    }

    public void PlayDeath()
    {
        audioManager.Play("Death");
    }

    public void PlayRun()
    {
        stopRun = audioManager.Play("Run");
    }

    public void StopRun()
    {
        if (stopRun != null)
        {
            stopRun();
        }
    }
}

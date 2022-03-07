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
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlaySlide()
    {
        audioManager.Play("Player_Slide");
    }

    public void PlayDodge()
    {
        audioManager.Play("Player_Dodge");
    }

    public void PlayJump()
    {
        audioManager.Play("Player_Jump");
    }

    public void PlayDeath()
    {
        audioManager.Play("Player_Death");
    }

    public void PlayRun()
    {
       stopRun = audioManager.Play("Player_Run");
    }

    public void StopRun()
    {
        stopRun();
    }
}

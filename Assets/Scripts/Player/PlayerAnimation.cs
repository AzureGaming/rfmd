using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    AudioManager audio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        animator.SetBool("MirrorAttack", false);
    }

    public void PlayRun()
    {
        animator.SetBool("Grounded", true);
    }

    public void PlayJump()
    {
        animator.SetTrigger("Jump");
    }

    public void PlaySlide(bool isSliding)
    {
        animator.SetBool("Slide", isSliding);
    }

    public void SetYVelocity(float val)
    {
        animator.SetFloat("Y Velocity", val);
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }

    public void PlayHurt()
    {
        animator.SetTrigger("Hurt");
        audio.Play("Player_Hurt");
    }

    public void PlayJumpAttack()
    {
        animator.SetTrigger("JumpAttack");
    }

    public void PlaySlideAttack()
    {
        animator.SetTrigger("SlideAttack");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
}

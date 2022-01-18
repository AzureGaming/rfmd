using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Attacked(string attackType);
    public static Attacked OnAttacked;

    public float jumpForce = 5f;
    public PlayerAnimation playerAnimation;

    bool queueJump = false;
    bool queueSlide = false;
    bool grounded = false;
    bool isDead = false;
    bool isInvincible = false;
    Coroutine jumpRoutine;
    Coroutine slideRoutine;
    Coroutine invincibleRoutine;
    Rigidbody2D rb;
    SpriteRenderer spriteR;

    private void OnEnable()
    {
        OnAttacked += TakeDamage;
    }

    private void OnDisable()
    {
        OnAttacked -= TakeDamage;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsActionValid())
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                queueJump = true;
                jumpRoutine = StartCoroutine(Jump());
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                queueSlide = true;
                slideRoutine = StartCoroutine(Slide());

            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                TakeDamage("");
            }
        }

        PlayAnimations();
    }

    private void FixedUpdate()
    {
        IsGrounded();
    }

    public void TakeDamage(string attackType)
    {
        if (invincibleRoutine != null)
        {
            return;
        }

        if (attackType == "HIGH") // player must slide
        {
            if (slideRoutine == null)
            {
                Debug.Log("You didn't slide. Took damage");
                playerAnimation.PlayHurt();
                FindObjectOfType<AudioManager>().Play("Player_Hurt");
                invincibleRoutine = StartCoroutine(ActivateInvincible());
            }
        }
        else if (attackType == "LOW") // player must jump
        {
            if (jumpRoutine == null)
            {
                Debug.Log("You didn't jump. Took damage");
                playerAnimation.PlayHurt();
                FindObjectOfType<AudioManager>().Play("Player_Hurt");
                invincibleRoutine = StartCoroutine(ActivateInvincible());
            }
        }
    }

    IEnumerator Slide()
    {
        spriteR.color = Color.blue;
        playerAnimation.PlaySlide(true);
        FindObjectOfType<AudioManager>().Play("Player_Slide");
        yield return new WaitForSeconds(0.5f);
        playerAnimation.PlaySlide(false);
        spriteR.color = Color.white;
        slideRoutine = null;
    }

    IEnumerator Jump()
    {
        float invincibleTime = 0.3f;
        float timeElapsed = 0f;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        playerAnimation.PlayJump();
        FindObjectOfType<AudioManager>().Play("Player_Jump");

        spriteR.color = Color.green;
        while (timeElapsed <= invincibleTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        spriteR.color = Color.white;
        jumpRoutine = null;
    }

    void Die()
    {
        isDead = true;
        playerAnimation.PlayDeath();
        FindObjectOfType<AudioManager>().Play("Player_Death");
    }

    bool IsActionValid()
    {
        return !isDead && grounded && slideRoutine == null && invincibleRoutine == null;
    }

    void IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up * 2, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 2f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void PlayAnimations()
    {
        playerAnimation.SetYVelocity(Mathf.Round(rb.velocity.y));
        playerAnimation.SetGrounded(grounded);
    }

    IEnumerator ActivateInvincible()
    {
        yield return new WaitForSeconds(0.5f);
        invincibleRoutine = null;
    }
}

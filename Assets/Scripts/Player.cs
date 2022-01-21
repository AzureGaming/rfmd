using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Attacked(string attackType);
    public static Attacked OnAttacked;

    public float jumpForce = 5f;
    public PlayerAnimation playerAnimation;
    public bool isDead { get; private set; } = false;

    bool queueJump = false;
    bool queueSlide = false;
    bool grounded = false;
    bool isInvincible = false;
    Coroutine jumpRoutine;
    Coroutine slideRoutine;
    Coroutine invincibleRoutine;
    Rigidbody2D rb;
    SpriteRenderer spriteR;

    GameManager gameManager;
    AudioManager audioManager;

    private void OnEnable()
    {
        OnAttacked += CheckDamage;
    }

    private void OnDisable()
    {
        OnAttacked -= CheckDamage;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        gameManager.isPlayerAlive = true;
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
                CheckDamage("");
            }
        }

        PlayAnimations();
    }

    private void FixedUpdate()
    {
        IsGrounded();
    }

    public void CheckDamage(string attackType)
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
                TakeDamage();
            }
        }
        else if (attackType == "LOW") // player must jump
        {
            if (jumpRoutine == null)
            {
                Debug.Log("You didn't jump. Took damage");
                TakeDamage();
            }
        }
    }

    void TakeDamage()
    {
        gameManager.lives--;
        if (gameManager.lives < 1)
        {
            Die();
            return;
        }
        playerAnimation.PlayHurt();
        audioManager.Play("Player_Hurt");
        invincibleRoutine = StartCoroutine(ActivateInvincible());
    }

    IEnumerator Slide()
    {
        spriteR.color = Color.blue;
        playerAnimation.PlaySlide(true);
        audioManager.Play("Player_Slide");
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
        audioManager.Play("Player_Jump");

        spriteR.color = Color.green;
        while (timeElapsed <= invincibleTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => grounded);
        spriteR.color = Color.white;
        jumpRoutine = null;
    }

    void Die()
    {
        if (!isDead) // prevent dying when dead
        {
            isDead = true;
            playerAnimation.PlayDeath();
            audioManager.Play("Player_Death");
            FindObjectOfType<GameManager>().isPlayerAlive = false;
        }
    }

    bool IsActionValid()
    {
        return !isDead && grounded && slideRoutine == null && invincibleRoutine == null;
    }

    void IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up * 3, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 3f, LayerMask.GetMask("Ground"));
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

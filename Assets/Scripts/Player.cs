using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Attacked(string attackType);
    public static Attacked OnAttacked;

    public PlayerAnimation animation;
    public PlayerAudio audio;

    public float jumpForce = 5f;

    public bool isDead { get; private set; } = false;

    [SerializeField] bool isGodMode = false;
    bool queueJump = false;
    bool queueSlide = false;
    bool grounded = false;
    bool isInvincible = false;
    bool isComboAttacking = false;
    Coroutine jumpRoutine;
    Coroutine slideRoutine;
    Coroutine invincibleRoutine;
    Rigidbody2D rb;
    SpriteRenderer spriteR;

    GameManager gameManager;
    DodgeTimingManager timingManager;

    Color origColor;

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
        timingManager = FindObjectOfType<DodgeTimingManager>();

        gameManager.isPlayerAlive = true;
    }

    private void Start()
    {
        audio.PlayRun();
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

    public void ComboAttack()
    {
        isComboAttacking = true;
        // PLACEHOLDER
        Color targetColor = Color.blue;

        origColor = spriteR.color;
        spriteR.color = targetColor;


    }

    public void EndComboAttack()
    {
        isComboAttacking = false;
        spriteR.color = origColor;
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
                Enemy.OnHighAttackHit?.Invoke();
                TakeDamage();
            }
            else
            {
                Dodge();
            }
        }
        else if (attackType == "LOW") // player must jump
        {
            if (jumpRoutine == null)
            {
                Debug.Log("You didn't jump. Took damage");
                Enemy.OnLowAttackHit?.Invoke();
                TakeDamage();
            }
            else
            {
                Dodge();
            }
        }
    }

    void Dodge()
    {
        audio.PlayDodge();
        //timingManager.DodgeInputted();
        gameManager.PlayerDodged();
    }

    void TakeDamage()
    {
        if (isGodMode)
        {
            return;
        }
        gameManager.PlayerHit();
        if (gameManager.GetLives() < 1)
        {
            Die();
            gameManager.PlayerDied();
            return;
        }
        animation.PlayHurt();
        invincibleRoutine = StartCoroutine(ActivateInvincible());
    }

    IEnumerator Slide()
    {
        animation.PlaySlide(true);
        audio.StopRun();
        audio.PlaySlide();

        yield return new WaitForSeconds(0.5f);

        animation.PlaySlide(false);
        slideRoutine = null;
        audio.PlayRun();
    }

    IEnumerator Jump()
    {
        float invincibleTime = 0.3f;
        float timeElapsed = 0f;
        audio.StopRun();

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animation.PlayJump();
        audio.PlayJump();
        while (timeElapsed <= invincibleTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => grounded);
        jumpRoutine = null;
        audio.PlayRun();
    }

    void Die()
    {
        if (!isDead) // prevent dying when dead
        {
            isDead = true;
            animation.PlayDeath();
            audio.PlayDeath();
            FindObjectOfType<GameManager>().isPlayerAlive = false;
        }
    }

    bool IsActionValid()
    {
        return !isDead && grounded && slideRoutine == null && invincibleRoutine == null && !isComboAttacking;
    }

    void IsGrounded()
    {
        float rayLength = 0.25f;
        Debug.DrawRay(transform.position, -Vector2.up * rayLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayLength, LayerMask.GetMask("Ground"));
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
        animation.SetYVelocity(Mathf.Round(rb.velocity.y));
        animation.SetGrounded(grounded);
    }

    IEnumerator ActivateInvincible()
    {
        yield return new WaitForSeconds(0.5f);
        invincibleRoutine = null;
    }
}
